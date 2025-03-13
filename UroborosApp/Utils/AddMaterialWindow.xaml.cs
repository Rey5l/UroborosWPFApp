using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using UroborosApp.Model;

namespace UroborosApp.Utils
{
    /// <summary>
    /// Логика взаимодействия для AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Window
    {
        private UroborosDBEntities _context;

        public AddMaterialWindow()
        {
            _context = new UroborosDBEntities();
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _context.Material_Category.ToList();
                CategoryComboBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}");
            }
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            var selectedCategory = CategoryComboBox.SelectedItem as Material_Category;

            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            var newMaterial = new Material
            {
                user_id = CurrentUser.Id,
                title = TitleTextBox.Text,
                content = DescriptionTextBox.Text,
                category = selectedCategory.name,
                created_at = DateTime.Now,
                category_id = selectedCategory.id,
            };

            try
            {
                _context.Material.Add(newMaterial);
                _context.SaveChanges();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении материала: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
