using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public partial class ActivityLogPage : Page
    {
        private Entities _context;
        public ObservableCollection<Activity_Log> ActivityLogData { get; set; }

        public ActivityLogPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            ActivityLogData = new ObservableCollection<Activity_Log>();
            Loaded += (s, e) => LoadActivityLogData();
        }

        private void LoadActivityLogData()
        {
            if (CurrentUser.Id == 0)
            {
                MessageBox.Show("Пользователь не авторизован");
                return;
            }

            var logs = _context.Activity_Log
                .Where(a => a.user_id == CurrentUser.Id)
                .ToList();

            ActivityLogData.Clear();
            foreach (var log in logs)
            {
                ActivityLogData.Add(log);
            }
            ActivityLogList.ItemsSource = ActivityLogData;
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

        private void NavigateToStatisticsPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new StatisticsPage());

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new TasksPage());


        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProfilePage());

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadActivityLogData();

            _context.Activity_Log.Add(new Activity_Log
            {
                user_id = CurrentUser.Id,
                log_datetime = DateTime.Now,
                action = "Обновлены данные журнала активности".Substring(0, Math.Min(255, "Обновлены данные журнала активности".Length))
            });
            _context.SaveChanges();
        }

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogData.Clear();
            _context.Activity_Log.RemoveRange(_context.Activity_Log.Where(a => a.user_id == CurrentUser.Id));
            _context.SaveChanges();
        }

        private void NavigateToLearningGoalsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }
    }
}