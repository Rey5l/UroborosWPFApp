using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {

        private UroborosDBEntities _context;

        public List<Material_Category> Categories { get; set; }

        public CategoriesPage()
        {
            InitializeComponent();
            _context = new UroborosDBEntities();
            LoadCategories();
        }


        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow();
            if (addCategoryWindow.ShowDialog() == true)
            {
                LoadCategories();

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
                Categories = _context.Material_Category.ToList();
                CategoryList.ItemsSource = null;
                CategoryList.ItemsSource = Categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void NavigateToHomePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}
