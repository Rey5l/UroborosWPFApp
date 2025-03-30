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
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        private UroborosDBEntities _context;

        public ObservableCollection<Material> Materials { get; set; }

        public HomePage()
        {
            InitializeComponent();
            _context = new UroborosDBEntities();
            LoadMaterials();
            LoadCategories();
        }

        private void AddNewMaterial_Click(object sender, RoutedEventArgs e)
        {
            var addMaterialWindow = new AddMaterialWindow();
            if (addMaterialWindow.ShowDialog() == true)
            {
                LoadMaterials();
            }
        }

        private void LoadMaterials()
        {
            try
            {
                Materials = new ObservableCollection<Material>(
                        _context.Material.Where(m => m.user_id == CurrentUser.Id)
                    );
                MaterialList.ItemsSource = Materials;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _context.Material_Category.ToList();
                CategoryList.ItemsSource = categories;
                var allCategories = new List<Material_Category> { new Material_Category { name = "Все" } };
                allCategories.AddRange(categories);
                CategoryList.ItemsSource = allCategories;

                CategoryList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}");
            }
        }

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoriesPage());
        }
    }
}
