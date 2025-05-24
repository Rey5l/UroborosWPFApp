using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public class ScheduleItemViewModel
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialTitle { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsReminder { get; set; }
    }

    public partial class ReviewSchedulePage : Page
    {
        private Entities _context;
        public ObservableCollection<ScheduleItemViewModel> ScheduleData { get; set; }

        public ReviewSchedulePage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            ScheduleData = new ObservableCollection<ScheduleItemViewModel>();
            Loaded += (s, e) => LoadSchedule();
        }

        private void LoadSchedule(string filter = "Все материалы")
        {
            try
            {
                ScheduleData.Clear(); 

                var remindersQuery = _context.Reminder
                    .Where(r => r.user_id == CurrentUser.Id);

                if (filter != "Все материалы")
                {
                    remindersQuery = remindersQuery.Where(r => r.Material != null && r.Material.title.Contains(filter));
                }

                var reminders = remindersQuery
                    .Select(r => new ScheduleItemViewModel
                    {
                        Id = r.id,
                        MaterialId = r.material_id,
                        MaterialTitle = r.Material != null ? r.Material.title : "Материал не найден",
                        DateTime = r.reminder_datetime,
                        IsReminder = true
                    })
                    .ToList();

                var repetitionsQuery = _context.Repetition
                    .Where(r => r.Material.user_id == CurrentUser.Id)
                    .GroupBy(r => r.material_id)
                    .Select(g => g.OrderByDescending(r => r.repetition_date).FirstOrDefault());

                if (filter != "Все материалы")
                {
                    repetitionsQuery = repetitionsQuery.Where(r => r.Material != null && r.Material.title.Contains(filter));
                }

                var lastRepetitions = repetitionsQuery.ToList();

                var futureRepetitions = new List<ScheduleItemViewModel>();
                foreach (var repetition in lastRepetitions)
                {
                    if (repetition == null) continue;

                    int intervalDays = repetition.recall_score >= 4 ? 7 : 1;
                    var nextRepetitionDate = repetition.repetition_date.AddDays(intervalDays);

                    if (nextRepetitionDate >= DateTime.Now)
                    {
                        futureRepetitions.Add(new ScheduleItemViewModel
                        {
                            Id = repetition.id,
                            MaterialId = (int) repetition.material_id,
                            MaterialTitle = repetition.Material != null ? repetition.Material.title : "Материал не найден",
                            DateTime = nextRepetitionDate,
                            IsReminder = false
                        });
                    }
                }

                // Объединяем списки
                var combinedList = reminders.Concat(futureRepetitions)
                    .OrderBy(item => item.DateTime)
                    .ToList();

                // Отладочный вывод
                foreach (var item in combinedList)
                {
                    Console.WriteLine($"Загружено: MaterialTitle={item.MaterialTitle}, DateTime={item.DateTime}, IsReminder={item.IsReminder}");
                }

                // Добавляем данные в ScheduleData
                foreach (var item in combinedList)
                {
                    ScheduleData.Add(item);
                }

                // Устанавливаем ItemsSource после добавления данных
                ScheduleList.ItemsSource = ScheduleData;

                // Логируем действие
                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = TruncateString($"Просмотр графика повторений (фильтр: {filter})", 255)
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки графика: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void MarkAsReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is ScheduleItemViewModel item)
            {
                try
                {
                    int recallScore = GetRecallScore();

                    var newRepetition = new Repetition
                    {
                        material_id = item.MaterialId,
                        repetition_date = DateTime.Now,
                        recall_score = recallScore
                    };
                    _context.Repetition.Add(newRepetition);

                    _context.SaveChanges();

                    ScheduleNextRepetition(item.MaterialId, newRepetition.repetition_date);

                    UpdateLearningStatistics(recallScore);

                    MessageBox.Show(
                        $"Материал успешно повторен!\n\nОценка запоминания: {recallScore}/5",
                        "Повторение завершено",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    LoadSchedule();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ScheduleNextRepetition(int materialId, DateTime lastRepetitionDate)
        {
            // Логика вычисления следующей даты повторения (без сохранения в таблицу)
            var lastRepetition = _context.Repetition
                .Where(r => r.material_id == materialId)
                .OrderByDescending(r => r.repetition_date)
                .FirstOrDefault();

            int intervalDays;
            if (lastRepetition != null && lastRepetition.recall_score.HasValue)
            {
                intervalDays = lastRepetition.recall_score >= 4 ? 7 : 1;
            }
            else
            {
                intervalDays = 3;
            }

            // Следующая дата повторения вычисляется, но не сохраняется в отдельную таблицу
            var nextRepetitionDate = lastRepetitionDate.AddDays(intervalDays);

            // Отладочный вывод
            Console.WriteLine($"Следующее повторение для material_id={materialId}: {nextRepetitionDate}");
        }

        private void UpdateLearningStatistics(int recallScore)
        {
            try
            {
                var stats = _context.Learning_Statistics.FirstOrDefault(s => s.user_id == CurrentUser.Id);

                if (stats == null)
                {
                    stats = new Learning_Statistics
                    {
                        user_id = CurrentUser.Id,
                        total_materials_learned = 0,
                        avg_recall_score = 0
                    };
                    _context.Learning_Statistics.Add(stats);
                }

                var allRepetitions = _context.Repetition
                    .Where(r => r.Material.user_id == CurrentUser.Id)
                    .ToList();

                if (allRepetitions.Any())
                {
                    stats.avg_recall_score = allRepetitions.Average(r => r.recall_score) * 20;
                }
                else
                {
                    stats.avg_recall_score = recallScore * 20;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статистики: {ex.Message}");
            }
        }

        private int GetRecallScore()
        {
            var dialog = new RecallScoreDialog();
            if (dialog.ShowDialog() == true)
            {
                return dialog.SelectedScore;
            }
            return 3;
        }

        private void Reschedule_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is ScheduleItemViewModel item)
            {
                var dialog = new RescheduleDialog(item.MaterialId);
                if (dialog.ShowDialog() == true)
                {
                    LoadSchedule();
                }
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterComboBox.SelectedItem is ComboBoxItem selectedFilter)
            {
                LoadSchedule(selectedFilter.Content.ToString());
            }
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadSchedule(FilterComboBox.SelectedItem is ComboBoxItem filter ? filter.Content.ToString() : "Все материалы");
        }

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProfilePage());

        private void NavigateToReminderPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ReminderPage());

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new CategoriesPage());

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new HomePage());

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProgressPage());

        private void NavigateToStatisticsPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new StatisticsPage());

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new TasksPage());

        private void NavigateToActivityLog(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ActivityLogPage());

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private void NavigateToLearningGoalsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }
    }
}