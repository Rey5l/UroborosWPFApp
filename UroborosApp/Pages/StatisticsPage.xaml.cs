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
    public partial class StatisticsPage : Page
    {
        private Entities _context;
        public ObservableCollection<Learning_Statistics> StatisticsData { get; set; }

        public StatisticsPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            StatisticsData = new ObservableCollection<Learning_Statistics>();
            Loaded += (s, e) => LoadStatisticsData();
        }

        private void LoadStatisticsData()
        {
            try
            {
                if (CurrentUser.Id == 0)
                {
                    MessageBox.Show("Пользователь не авторизован");
                    return;
                }

                var stats = _context.Learning_Statistics
                    .Where(s => s.user_id == CurrentUser.Id)
                    .ToList();

                if (!stats.Any())
                {
                    MessageBox.Show($"Нет данных о статистике для пользователя с ID {CurrentUser.Id}.");
                    StatisticsData.Clear();
                    StatisticsList.ItemsSource = StatisticsData;
                    UpdateUI(0, 0);
                    return;
                }

                StatisticsData.Clear();
                foreach (var item in stats)
                {
                    StatisticsData.Add(item);
                }
                StatisticsList.ItemsSource = StatisticsData;

                var totalMaterials = stats.Sum(s => s.total_materials_learned); 
                var avgRecallScore = stats.Average(s => s.avg_recall_score ?? 0);
                UpdateUI((int)totalMaterials, avgRecallScore);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статистики: {ex.Message}\n\n{ex.InnerException?.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void UpdateUI(int totalMaterials, double avgRecallScore)
        {
            TotalMaterialsText.Text = totalMaterials.ToString();
            AvgRecallScoreText.Text = $"{avgRecallScore:F2}%";
            RecallScoreBar.Value = avgRecallScore;
        }

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new HomePage());

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new CategoriesPage());

        private void NavigateToReminderPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ReminderPage());

        private void NavigateToReviewSchedulePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ReviewSchedulePage());

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProgressPage());

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProfilePage());

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new TasksPage());


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