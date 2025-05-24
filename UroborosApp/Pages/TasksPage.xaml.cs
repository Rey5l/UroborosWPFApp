using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public class BoolToShowHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Скрыть" : "Показать";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class TasksPage : Page
    {
        private Entities _context;
        public ObservableCollection<Tasks> TasksData { get; set; }

        public TasksPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            TasksData = new ObservableCollection<Tasks>();
            Loaded += (s, e) => LoadTasksData();
            LoadMaterialFilter();
        }

        private void LoadTasksData(string searchText = null)
        {
            try
            {
                if (CurrentUser.Id == 0)
                {
                    MessageBox.Show("Пользователь не авторизован");
                    return;
                }

                var tasks = _context.Tasks
                    .Include(t => t.Material)
                    .Where(t => t.Material.user_id == CurrentUser.Id)
                    .ToList();


                if (!string.IsNullOrEmpty(searchText))
                {
                    tasks = tasks.Where(t => t.question.Contains(searchText)).ToList();

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Поиск задач: {searchText}".Substring(0, Math.Min(255, $"Поиск задач: {searchText}".Length))
                    });
                    _context.SaveChanges();
                }

                TasksData.Clear();
                foreach (var task in tasks)
                {
                    task.IsAnswerVisible = false;
                    TasksData.Add(task);
                }
                TasksList.ItemsSource = TasksData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}\n\n{ex.InnerException?.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void LoadMaterialFilter()
        {
            try
            {
                var materials = _context.Material
                    .Where(m => m.user_id == CurrentUser.Id)
                    .Select(m => new { m.id, m.title })
                    .ToList();

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = "Загружены материалы для фильтра задач".Substring(0, Math.Min(255, "Загружены материалы для фильтра задач".Length))
                });
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки материалов: {ex.Message}");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadTasksData(SearchBox.Text);
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            LoadTasksData();

            _context.Activity_Log.Add(new Activity_Log
            {
                user_id = CurrentUser.Id,
                log_datetime = DateTime.Now,
                action = "Сброшены фильтры на странице задач".Substring(0, Math.Min(255, "Сброшены фильтры на странице задач".Length))
            });
            _context.SaveChanges();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TaskDialog(null); 
            if (dialog.ShowDialog() == true)
            {
                _context.Tasks.Add(dialog.Task);
                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = $"Добавлена новая задача: {dialog.Task.question}".Substring(0, Math.Min(255, $"Добавлена новая задача: {dialog.Task.question}".Length))
                });
                _context.SaveChanges();
                LoadTasksData();
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Tasks task)
            {
                var dialog = new TaskDialog(task);
                if (dialog.ShowDialog() == true)
                {
                    _context.Activity_Log.Add(new Activity_Log    
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Изменена задача: {task.question}".Substring(0, Math.Min(255, $"Изменена задача: {task.question}".Length))
                    });
                    _context.SaveChanges();
                    LoadTasksData();
                }
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Tasks task)
            {
                if (MessageBox.Show("Удалить задачу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Tasks.Remove(task);
                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Удалена задача: {task.question}".Substring(0, Math.Min(255, $"Удалена задача: {task.question}".Length))
                    });
                    _context.SaveChanges();
                    LoadTasksData();
                }
            }
        }

        private void ShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Tasks task)
            {
                task.IsAnswerVisible = !task.IsAnswerVisible;
                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = $"{(task.IsAnswerVisible ? "Показан" : "Скрыт")} ответ на задачу: {task.question}".Substring(0, Math.Min(255, $"{(task.IsAnswerVisible ? "Показан" : "Скрыт")} ответ на задачу: {task.question}".Length))
                });
                _context.SaveChanges();
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

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProgressPage());

        private void NavigateToStatisticsPage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new StatisticsPage());

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ProfilePage());

        private void NavigateToActivityLog(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ActivityLogPage());

        private void NavigateToLearningGoalsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }
    }
}