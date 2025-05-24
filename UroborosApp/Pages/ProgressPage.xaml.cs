using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public partial class ProgressPage : Page
    {
        private Entities _context;
        public ObservableCollection<Mastery_Level> ProgressData { get; set; }

        public ProgressPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            ProgressData = new ObservableCollection<Mastery_Level>();
            Loaded += (s, e) => LoadProgressData();
        }

        private void LoadProgressData(string levelFilter = "Все уровни")
        {
            try
            {
                if (CurrentUser.Id == 0)
                {
                    MessageBox.Show("Пользователь не авторизован");
                    return;
                }

                var progressData = _context.Mastery_Level
                    .Include(m => m.Material)
                    .Where(m => m.Material != null && m.Material.user_id == CurrentUser.Id)
                    .ToList();

                if (!progressData.Any())
                {
                    MessageBox.Show($"Нет данных о прогрессе для пользователя с ID {CurrentUser.Id}.");
                    ProgressData.Clear();
                    ProgressList.ItemsSource = ProgressData;
                    return;
                }

                var nullMaterials = progressData.Where(m => m.Material == null).ToList();
                if (nullMaterials.Any())
                {
                    var nullMaterialIds = string.Join(", ", nullMaterials.Select(m => m.material_id));
                    MessageBox.Show($"Найдено {nullMaterials.Count} записей без материала. Material IDs: {nullMaterialIds}. Проверьте связь в базе данных.");
                }


                if (!string.IsNullOrWhiteSpace(levelFilter) && levelFilter != "Все уровни")
                {
                    switch (levelFilter)
                    {
                        case "Новичок (1-2)":
                            progressData = progressData.Where(m => m.level == "1" || m.level == "2").ToList();
                            break;
                        case "Средний (3-4)":
                            progressData = progressData.Where(m => m.level == "3" || m.level == "4").ToList();
                            break;
                        case "Эксперт (5)":
                            progressData = progressData.Where(m => m.level == "5").ToList();
                            break;
                    }
                }

                progressData = progressData
                    .Select(m => new
                    {
                        Entity = m,
                        Level = int.TryParse(m.level, out int l) ? l : 0
                    })
                    .OrderByDescending(x => x.Level)
                    .ThenByDescending(x => x.Entity.last_updated)
                    .Select(x => x.Entity)
                    .ToList();

                ProgressData.Clear();
                foreach (var item in progressData)
                {
                    ProgressData.Add(item);
                }
                ProgressList.ItemsSource = ProgressData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки прогресса: {ex.Message}\n\n{ex.InnerException?.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void IncreaseLevel_Click(object sender, RoutedEventArgs e)
        {
            if (!(((Button)sender).DataContext is Mastery_Level mastery)) return;

            try
            {
                if (mastery.Material == null)
                {
                    MessageBox.Show("Ошибка: материал не найден");
                    return;
                }

                if (!int.TryParse(mastery.level, out int currentLevel))
                {
                    currentLevel = 1;
                }

                if (currentLevel < 5)
                {
                    mastery.level = (currentLevel + 1).ToString();
                    mastery.last_updated = DateTime.Now;
                    _context.SaveChanges();

                    if (mastery.level == "5")
                    {
                        UpdateLearningStatisticsOnMastery();
                    }

                    LoadProgressData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при повышении уровня: {ex.Message}");
            }
        }

        private void UpdateLearningStatisticsOnMastery()
        {
            try
            {
                var stats = _context.Learning_Statistics
                    .FirstOrDefault(s => s.user_id == CurrentUser.Id);

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

                stats.total_materials_learned = (stats.total_materials_learned) + 1;

                var allRepetitions = _context.Repetition
                    .Where(r => r.Material.user_id == CurrentUser.Id)
                    .ToList();

                if (allRepetitions.Any())
                {
                    stats.avg_recall_score = allRepetitions.Average(r => r.recall_score) * 20;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статистики: {ex.Message}");
            }
        }

        private void DecreaseLevel_Click(object sender, RoutedEventArgs e)
        {
            if (!(((Button)sender).DataContext is Mastery_Level mastery)) return;

            try
            {
                if (mastery.Material == null)
                {
                    MessageBox.Show("Ошибка: материал не найден");
                    return;
                }

                if (!int.TryParse(mastery.level, out int currentLevel))
                {
                    currentLevel = 1;
                }

                if (currentLevel > 1)
                {
                    mastery.level = (currentLevel - 1).ToString();
                    mastery.last_updated = DateTime.Now;
                    _context.SaveChanges();
                    LoadProgressData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при понижении уровня: {ex.Message}");
            }
        }

       

        private void LevelFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LevelFilterComboBox.SelectedItem is ComboBoxItem selected)
            {
                LoadProgressData(selected.Content.ToString());
            }
        }

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new HomePage());

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new CategoriesPage());

        private void NavigateToReminderPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ReminderPage());

        private void NavigateToReviewSchedulePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ReviewSchedulePage());

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProfilePage());

        private void NavigateToStatisticsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StatisticsPage());
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            LevelFilterComboBox.SelectedItem = -1;
            LoadProgressData();
        }

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TasksPage());
        }

        private void NavigateToActivityLog(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ActivityLogPage());
        }

        private void NavigateToLearningGoalsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }
    }
}