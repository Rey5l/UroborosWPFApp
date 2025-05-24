using System;
using System.Linq;
using System.Windows;

using UroborosApp.Model;

namespace UroborosApp.Utils
{
    public partial class EditReminderWindow : Window
    {
        private Entities _context;
        private int _reminderId;
        private Reminder _currentReminder;

        public EditReminderWindow(int reminderId)
        {
            InitializeComponent();
            _context = Entities.GetContext();
            _reminderId = reminderId;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _currentReminder = _context.Reminder.Find(_reminderId);

                MaterialComboBox.ItemsSource = _context.Material
                    .Where(m => m.user_id == CurrentUser.Id)
                    .ToList();

                if (_currentReminder != null)
                {
                    MaterialComboBox.SelectedValue = _currentReminder.material_id;
                    DatePicker.SelectedDate = _currentReminder.reminder_datetime;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                Close();
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

                _currentReminder.material_id = material.id;
                _currentReminder.reminder_datetime = reminderDateTime;

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