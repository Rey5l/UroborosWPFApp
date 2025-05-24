using System;
using System.Linq;
using System.Windows;

using UroborosApp.Model;

namespace UroborosApp.Utils
{
    public partial class AddReminderWindow : Window
    {
        private Entities _context;

        public AddReminderWindow()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            LoadMaterials();
            DatePicker.SelectedDate = DateTime.Today;
        }

        private void LoadMaterials()
        {
            try
            {
                MaterialComboBox.ItemsSource = _context.Material
                    .Where(m => m.user_id == CurrentUser.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки материалов: {ex.Message}");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialComboBox.SelectedItem == null ||
                DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            try
            {
                var material = (Material)MaterialComboBox.SelectedItem;
                var reminderDateTime = DatePicker.SelectedDate.Value.Date;

                var reminder = new Reminder
                {
                    user_id = CurrentUser.Id,
                    material_id = material.id,
                    reminder_datetime = reminderDateTime
                };

                _context.Reminder.Add(reminder);
                _context.SaveChanges();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}