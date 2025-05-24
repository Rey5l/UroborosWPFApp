using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Entities _context;

        public ObservableCollection<Material> Materials { get; set; }

        public HomePage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            Materials = new ObservableCollection<Material>();
            LoadCategories();
            LoadMaterials();
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var addMaterialWindow = new AddMaterialWindow();
            if (addMaterialWindow.ShowDialog() == true)
            {
                LoadMaterials();
                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = "Добавлен новый материал через AddMaterial".Substring(0, Math.Min(255, "Добавлен новый материал через AddMaterial".Length))
                });
                _context.SaveChanges();
            }
        }

        private void LoadCategories()
        {
            var categories = _context.Material_Category
                .Where(x => x.user_id == CurrentUser.Id)
                .Distinct()
                .ToList();
            SortComboBox.ItemsSource = categories;
        }

        private void LoadMaterials()
        {
            var materials = _context.Material
                .Where(x => x.user_id == CurrentUser.Id)
                .ToList();

            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                materials = materials.Where(x =>
                    x.title.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.content.ToLower().Contains(SearchBox.Text.ToLower()))
                    .ToList();

                _context.Activity_Log.Add(new Activity_Log
                {
                    user_id = CurrentUser.Id,
                    log_datetime = DateTime.Now,
                    action = $"Поиск материалов: {SearchBox.Text}".Substring(0, Math.Min(255, $"Поиск материалов: {SearchBox.Text}".Length))
                });
                _context.SaveChanges();
            }

            if (SortComboBox.SelectedItem != null)
            {
                var selectedCategory = SortComboBox.SelectedItem as Material_Category;
                if (selectedCategory != null)
                {
                    materials = materials.Where(x => x.category == selectedCategory.name).ToList();

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Фильтрация материалов по категории: {selectedCategory.name}".Substring(0, Math.Min(255, $"Фильтрация материалов по категории: {selectedCategory.name}".Length))
                    });
                    _context.SaveChanges();
                }
            }

            Materials.Clear();
            foreach (var material in materials)
            {
                Materials.Add(material);
            }
            MaterialList.ItemsSource = Materials;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadMaterials();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMaterials();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            SortComboBox.SelectedItem = null;
            LoadMaterials();
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Material material)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить материал '{material.title}'?", "Подтверждение удаления",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.Material.Remove(material);
                        _context.SaveChanges();

                        LoadMaterials();

                        _context.Activity_Log.Add(new Activity_Log
                        {
                            user_id = CurrentUser.Id,
                            log_datetime = DateTime.Now,
                            action = $"Удалён материал: {material.title}".Substring(0, Math.Min(255, $"Удалён материал: {material.title}".Length))
                        });
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении материала: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void MaterialList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Material material)
            {
                EditMaterial(material);
            }
        }

        private void EditMaterial(Material material)
        {
            var editMaterialWindow = new EditMaterialWindow(material); // Предполагаем, что есть EditMaterialWindow
            if (editMaterialWindow.ShowDialog() == true)
            {
                try
                {
                    _context.SaveChanges();

                    LoadMaterials();

                    _context.Activity_Log.Add(new Activity_Log
                    {
                        user_id = CurrentUser.Id,
                        log_datetime = DateTime.Now,
                        action = $"Отредактирован материал: {material.title}".Substring(0, Math.Min(255, $"Отредактирован материал: {material.title}".Length))
                    });
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при редактировании материала: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoriesPage());
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

        private void NavigateToLearningGoals(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }
    }
}