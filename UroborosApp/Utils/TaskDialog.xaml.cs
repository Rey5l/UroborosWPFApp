using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Utils
{
    public partial class TaskDialog : Window
    {
        private Entities _context;
        public Tasks Task { get; set; }

        public TaskDialog(Tasks task = null)
        {
            InitializeComponent();
            _context = Entities.GetContext();

            Task = task ?? new Tasks
            {
                material_id = 0,
                question = string.Empty,
                answer = string.Empty
            };

            DataContext = Task;

            LoadMaterials();
        }

        private void LoadMaterials()
        {
            try
            {
                var materials = _context.Material
                    .Where(m => m.user_id == CurrentUser.Id)
                    .ToList();

                MaterialComboBox.ItemsSource = materials;
                MaterialComboBox.SelectedIndex = materials.FindIndex(m => m.id == Task.material_id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки материалов: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите материал.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                MessageBox.Show("Вопрос не может быть пустым.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(AnswerTextBox.Text))
            {
                MessageBox.Show("Ответ не может быть пустым.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Task.material_id = (MaterialComboBox.SelectedItem as Material).id;
            Task.question = QuestionTextBox.Text;
            Task.answer = AnswerTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}