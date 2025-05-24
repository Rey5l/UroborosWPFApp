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
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private Entities _context;

        public AddCategoryWindow()
        {
            InitializeComponent();
            _context = Entities.GetContext();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }


            var newCategory = new Material_Category
            {
                user_id = CurrentUser.Id,
                name = TitleTextBox.Text,
                description = DescriptionTextBox.Text,
            };

            try
            {
                _context.Material_Category.Add(newCategory);
                _context.SaveChanges();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении категории: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
