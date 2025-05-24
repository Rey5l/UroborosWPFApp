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
    /// Логика взаимодействия для EditCategoryWindow.xaml
    /// </summary>
    public partial class EditCategoryWindow : Window
    {
        private Entities _context;

        private Material_Category _category;

        public EditCategoryWindow(int categoryId)
        {
            InitializeComponent();
            _context = new Entities();
            _category = _context.Material_Category.Find(categoryId);
            NameTextBox.Text = _category.name;
            DescriptionTextBox.Text = _category.description;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Название категории не может быть пустым");
                return;
            }

            _category.name = NameTextBox.Text;
            _category.description = DescriptionTextBox.Text;

            try
            {
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
