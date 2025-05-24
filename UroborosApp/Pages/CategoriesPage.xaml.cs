using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {

        private Entities _context;

        public List<Material_Category> Categories { get; set; }

        public CategoriesPage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            LoadCategories();
        }


        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow();
            if (addCategoryWindow.ShowDialog() == true)
            {
                LoadCategories();

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = "Добавлена новая категория".Substring(0, Math.Min(255, "Добавлена новая категория".Length))
                });
                _context.SaveChanges();

            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoryList.SelectedItem as Material_Category;

            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию для редактирования.");
                return;
            }

            var editCategoryWindow = new EditCategoryWindow(selectedCategory.id);
            if (editCategoryWindow.ShowDialog() == true)
            {
                LoadCategories();

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = $"Изменена категория: {selectedCategory.name}".Substring(0, Math.Min(255, $"Изменена категория: {selectedCategory.name}".Length))
                });
                _context.SaveChanges();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoryList.SelectedItem as Material_Category;

            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию для удаления.");
                return;
            }

            try
            {
                _context.Material_Category.Remove(selectedCategory);

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = $"Удалена категория: {selectedCategory.name}".Substring(0, Math.Min(255, $"Удалена категория: {selectedCategory.name}".Length))
                });

                _context.SaveChanges();
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении категории: {ex.Message}");
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _context.Material_Category
                    .Where(c => c.user_id == CurrentUser.Id)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(SearchCategoryBox.Text))
                {
                    var searchText = SearchCategoryBox.Text.ToLower();
                    categories = categories.Where(c =>
                        c.name.ToLower().Contains(searchText) ||
                        c.description.ToLower().Contains(searchText)).ToList();

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Поиск категорий: {SearchCategoryBox.Text}".Substring(0, Math.Min(255, $"Поиск категорий: {SearchCategoryBox.Text}".Length))
                    });
                    _context.SaveChanges();
                }

                if (SortComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    switch (selectedItem.Content.ToString())
                    {
                        case "А-Я":
                            categories = categories.OrderBy(c => c.name).ToList();
                            break;
                        case "Я-А":
                            categories = categories.OrderByDescending(c => c.name).ToList();
                            break;
                        default:
                            break; 
                    }

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Сортировка категорий: {selectedItem.Content}".Substring(0, Math.Min(255, $"Сортировка категорий: {selectedItem.Content}".Length))
                    });
                    _context.SaveChanges();
                }

                Categories = categories;
                CategoryList.ItemsSource = Categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void SearchCategoryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadCategories();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCategories();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchCategoryBox.Text = "";
            SortComboBox.SelectedIndex = 0;
        }

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }

        private void NavigateToReminderPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReminderPage());
        }

        private void NavigateToReviewSchedulePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReviewSchedulePage());
        }

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProgressPage());
        }

        private void NavigateToStatistics(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StatisticsPage());
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
