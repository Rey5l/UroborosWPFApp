using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public class BooleanToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? "Выполнена" : "Не выполнена";
            }
            return "Не определено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public partial class LearningGoalsPage : Page
    {
        private readonly Entities _context;
        public ObservableCollection<Learning_Goals> GoalsData { get; set; }

        public LearningGoalsPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            GoalsData = new ObservableCollection<Learning_Goals>();
            Loaded += (s, e) => LoadGoalsData();
        }

        private void LoadGoalsData(string statusFilter = "Все цели", string searchText = null)
        {
            try
            {
                if (CurrentUser.Id == 0)
                {
                    MessageBox.Show("Пользователь не авторизован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var goalsQuery = _context.Learning_Goals
                    .Where(g => g.User_id == CurrentUser.Id);

                var logs = new List<Activity_Log>();

                switch (statusFilter)
                {
                    case "Выполненные":
                        goalsQuery = goalsQuery.Where(g => g.Is_completed);
                        logs.Add(new Activity_Log
                        {
                            user_id = CurrentUser.Id,
                            log_datetime = DateTime.Now,
                            action = TruncateString("Фильтрация целей: Выполненные", 255)
                        });
                        break;
                    case "Невыполненные":
                        goalsQuery = goalsQuery.Where(g => !g.Is_completed);
                        logs.Add(new Activity_Log
                        {
                            user_id = CurrentUser.Id,
                            log_datetime = DateTime.Now,
                            action = TruncateString("Фильтрация целей: Невыполненные", 255)
                        });
                        break;
                }

                if (!string.IsNullOrEmpty(searchText))
                {
                    goalsQuery = goalsQuery.Where(g => g.Title.ToLower().Contains(searchText.ToLower()));
                    logs.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = TruncateString($"Поиск целей: {searchText}", 255)
                    });
                }

                var goals = goalsQuery
                    .OrderByDescending(g => g.Created_Date)
                    .ToList();

                GoalsData.Clear();
                foreach (var goal in goals)
                {
                    GoalsData.Add(goal);
                }
                GoalsList.ItemsSource = GoalsData;

                if (logs.Any())
                {
                    foreach (var log in logs)
                    {
                        _context.Activity_Log.Add(log);
                    }
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки целей: {ex.Message}\n{ex.InnerException?.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusFilterComboBox.SelectedItem is ComboBoxItem selected && selected.Content != null)
            {
                LoadGoalsData(selected.Content.ToString(), SearchBox.Text);
            }
            else
            {
                LoadGoalsData("Все цели", SearchBox.Text);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string statusFilter = StatusFilterComboBox.SelectedItem is ComboBoxItem selected && selected.Content != null
                ? selected.Content.ToString()
                : "Все цели";
            LoadGoalsData(statusFilter, SearchBox.Text);
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            StatusFilterComboBox.SelectedIndex = 0;
            SearchBox.Text = string.Empty;
            LoadGoalsData();
        }

        private void AddGoal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new LearningGoalsDialog(null);
                if (dialog.ShowDialog() == true && dialog.LearningGoal != null)
                {
                    Console.WriteLine($"Добавление цели: User_id={dialog.LearningGoal.User_id}, Title={dialog.LearningGoal.Title}, " +
                                      $"Description={dialog.LearningGoal.Description}, Target_Date={dialog.LearningGoal.Target_Date}, " +
                                      $"Is_completed={dialog.LearningGoal.Is_completed}, Created_Date={dialog.LearningGoal.Created_Date}");

                    if (string.IsNullOrWhiteSpace(dialog.LearningGoal.Title))
                    {
                        MessageBox.Show("Название цели не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    _context.Learning_Goals.Add(dialog.LearningGoal);
                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = TruncateString($"Добавлена цель: {dialog.LearningGoal.Title}", 255)
                    });
                    _context.SaveChanges();
                    LoadGoalsData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при добавлении цели: {ex.Message}\n{ex.InnerException?.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void EditGoal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Button)sender).DataContext is Learning_Goals goal)
                {
                    var dialog = new LearningGoalsDialog(goal);
                    if (dialog.ShowDialog() == true)
                    {
                        Console.WriteLine($"Редактирование цели: User_id={goal.User_id}, Title={goal.Title}, " +
                                          $"Description={goal.Description}, Target_Date={goal.Target_Date}, " +
                                          $"Is_completed={goal.Is_completed}, Created_Date={goal.Created_Date}");

                        _context.Activity_Log.Add(new Activity_Log
                        {
                            user_id = CurrentUser.Id,
                            log_datetime = DateTime.Now,
                            action = TruncateString($"Изменена цель: {goal.Title}", 255)
                        });
                        _context.SaveChanges();
                        LoadGoalsData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при редактировании цели: {ex.Message}\n{ex.InnerException?.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void CompleteGoal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Button)sender).DataContext is Learning_Goals goal)
                {
                    goal.Is_completed = true;
                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = TruncateString($"Цель выполнена: {goal.Title}", 255)
                    });
                    _context.SaveChanges();
                    LoadGoalsData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при выполнении цели: {ex.Message}\n{ex.InnerException?.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void DeleteGoal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Button)sender).DataContext is Learning_Goals goal)
                {
                    if (MessageBox.Show("Удалить цель?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _context.Learning_Goals.Remove(goal);
                        _context.Activity_Log.Add(new Activity_Log
                        {
                            user_id = CurrentUser.Id,
                            log_datetime = DateTime.Now,
                            action = TruncateString($"Удалена цель: {goal.Title}", 255)
                        });
                        _context.SaveChanges();
                        LoadGoalsData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при удалении цели: {ex.Message}\n{ex.InnerException?.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CategoriesPage());
        }

        private void NavigateToReminderPage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReminderPage());
        }

        private void NavigateToReviewSchedulePage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReviewSchedulePage());
        }

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ProgressPage());
        }

        private void NavigateToStatisticsPage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new StatisticsPage());
        }

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TasksPage());
        }

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ProfilePage());
        }

        private void NavigateToActivityLog(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ActivityLogPage());
        }
    }
}