using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using UroborosApp.Model;
using UroborosApp.Utils;
using System.Collections.Generic;

namespace UroborosApp.Pages
{
    public partial class ReminderPage : Page
    {
        private Entities _context;
        public ObservableCollection<Reminder> Reminders { get; set; }

        public ReminderPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            Reminders = new ObservableCollection<Reminder>();
            LoadReminders();
        }

        private void LoadReminders()
        {
            try
            {
                if (CurrentUser.Username == null || CurrentUser.Id <= 0)
                {
                    MessageBox.Show("Текущий пользователь не определён");
                    return;
                }

                var reminders = _context.Reminder
                    .Include(r => r.Material)
                    .Where(r => r.user_id == CurrentUser.Id)
                    .ToList();

                if (!reminders.Any())
                {
                    MessageBox.Show($"Нет напоминаний для пользователя с ID {CurrentUser.Id}.");
                    Reminders.Clear();
                    RemindersList.ItemsSource = Reminders;
                    return;
                }

                var nullMaterials = reminders.Where(r => r.Material == null).ToList();
                if (nullMaterials.Any())
                {
                    var nullMaterialIds = string.Join(", ", nullMaterials.Select(r => r.material_id));
                    MessageBox.Show($"Найдено {nullMaterials.Count} напоминаний без материала. Material IDs: {nullMaterialIds}. Проверьте связь в базе данных.");
                }

                var now = DateTime.Now;

                if (StatusFilter.SelectedItem is ComboBoxItem status)
                {
                    switch (status.Content.ToString())
                    {
                        case "Сегодня":
                            reminders = reminders.Where(r => r.reminder_datetime.Date == now.Date).ToList();
                            break;
                        case "Предстоящие":
                            reminders = reminders.Where(r => r.reminder_datetime > now).ToList();
                            break;
                        case "Просроченные":
                            reminders = reminders.Where(r => r.reminder_datetime < now).ToList();
                            break;
                        case "Следующие 7 дней":
                            reminders = reminders.Where(r =>
                                r.reminder_datetime > now &&
                                r.reminder_datetime <= now.AddDays(7)).ToList();
                            break;
                        default:
                            break;
                    }

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Фильтрация напоминаний: {status.Content}".Substring(0, Math.Min(255, $"Фильтрация напоминаний: {status.Content}".Length))
                    });
                    _context.SaveChanges();
                }

                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    var searchText = SearchBox.Text.ToLower();
                    reminders = reminders.Where(r =>
                        r.Material != null && r.Material.title.ToLower().Contains(searchText)).ToList();

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Поиск напоминаний: {SearchBox.Text}".Substring(0, Math.Min(255, $"Поиск напоминаний: {SearchBox.Text}".Length))
                    });
                    _context.SaveChanges();
                }

                reminders = reminders.OrderBy(r => r.reminder_datetime).ToList();

                Reminders.Clear();
                foreach (var reminder in reminders)
                {
                    Reminders.Add(reminder);
                }
                RemindersList.ItemsSource = Reminders;

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = "Загружен список напоминаний".Substring(0, Math.Min(255, "Загружен список напоминаний".Length))
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => LoadReminders();

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => LoadReminders();

        private void GroupByDateCheck_Changed(object sender, RoutedEventArgs e)
            => LoadReminders();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            StatusFilter.SelectedIndex = -1;
            LoadReminders();

            _context.Activity_Log.Add(new Activity_Log
            {
                user_id = CurrentUser.Id,
                log_datetime = DateTime.Now,
                action = "Сброшены фильтры на странице напоминаний".Substring(0, Math.Min(255, "Сброшены фильтры на странице напоминаний".Length))
            });
            _context.SaveChanges();
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadReminders();
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddReminderWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadReminders();

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = "Добавлено новое напоминание".Substring(0, Math.Min(255, "Добавлено новое напоминание".Length))
                });
                _context.SaveChanges();
            }
        }

        private void EditReminder_Click(object sender, RoutedEventArgs e)
        {
            var reminder = (sender as Button)?.DataContext as Reminder;
            if (reminder != null)
            {
                var editWindow = new EditReminderWindow(reminder.id);
                if (editWindow.ShowDialog() == true)
                {
                    LoadReminders();
                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Изменено напоминание ID: {reminder.id}".Substring(0, Math.Min(255, $"Изменено напоминание ID: {reminder.id}".Length))
                    });
                    _context.SaveChanges();
                }
            }
        }

        private void MarkAsCompleted_Click(object sender, RoutedEventArgs e)
        {
            var reminder = (sender as Button)?.DataContext as Reminder;
            if (reminder != null)
            {
                try
                {
                    int recallScore = GetRecallScore();

                    var repetition = new Repetition
                    {
                        material_id = reminder.material_id,
                        repetition_date = DateTime.Now,
                        recall_score = recallScore
                    };
                    _context.Repetition.Add(repetition);

                    _context.Reminder.Remove(reminder);

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Напоминание ID: {reminder.id} отмечено как выполненное с оценкой {recallScore}".Substring(0, Math.Min(255, $"Напоминание ID: {reminder.id} отмечено как выполненное с оценкой {recallScore}".Length))
                    });
                    _context.SaveChanges();

                    ScheduleNextRepetition(reminder.material_id, repetition.repetition_date);

                    LoadReminders();

                    MessageBox.Show(
                        $"Напоминание успешно выполнено!\n\nОценка запоминания: {recallScore}/5",
                        "Выполнено",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void DeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            var reminder = (sender as Button)?.DataContext as Reminder;
            if (reminder != null)
            {
                var result = MessageBox.Show("Удалить это напоминание?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int recallScore = GetRecallScore();

                        var repetition = new Repetition
                        {
                            material_id = reminder.material_id,
                            repetition_date = DateTime.Now,
                            recall_score = recallScore
                        };
                        _context.Repetition.Add(repetition);

                        _context.Reminder.Remove(reminder);

                        _context.Activity_Log.Add(new Activity_Log
                        {
                            user_id = CurrentUser.Id,
                            log_datetime = DateTime.Now,
                            action = $"Удалено напоминание ID: {reminder.id} с оценкой {recallScore}".Substring(0, Math.Min(255, $"Удалено напоминание ID: {reminder.id} с оценкой {recallScore}".Length))
                        });
                        _context.SaveChanges();

                        ScheduleNextRepetition(reminder.material_id, repetition.repetition_date);

                        LoadReminders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}");
                    }
                }
            }
        }

        private int GetRecallScore()
        {
            var dialog = new Window
            {
                Title = "Оценка запоминания",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new StackPanel
                {
                    Margin = new Thickness(10),
                    Children =
                    {
                        new TextBlock { Text = "Оцените запоминание (1-5):" },
                        new ComboBox
                        {
                            Name = "ScoreComboBox",
                            ItemsSource = new List<int> { 1, 2, 3, 4, 5 },
                            SelectedIndex = 0,
                            Margin = new Thickness(0, 10, 0, 10)
                        },
                        new Button
                        {
                            Content = "Подтвердить",
                            Width = 100,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Tag = "ConfirmButton"
                        }
                    }
                }
            };

            var confirmButton = (dialog.Content as StackPanel).Children.OfType<Button>().First(b => b.Tag?.ToString() == "ConfirmButton");
            confirmButton.Click += (s, e) =>
            {
                dialog.DialogResult = true;
                dialog.Close();
            };

            if (dialog.ShowDialog() == true)
            {
                var scoreComboBox = (dialog.Content as StackPanel).Children.OfType<ComboBox>().First();
                return (int)scoreComboBox.SelectedItem;
            }

            return 3;
        }

        private void ScheduleNextRepetition(int materialId, DateTime lastRepetitionDate)
        {
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

            _context.SaveChanges();
        }

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new HomePage());

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new CategoriesPage());

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProfilePage());

        private void NavigateToReviewSchedulePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ReviewSchedulePage());

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProgressPage());

        private void NavigateToStatisticsPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new StatisticsPage());

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new TasksPage());

        private void NavigateToActivityLog(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ActivityLogPage());

        private void NavigateToLearningGoalsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }
    }
}