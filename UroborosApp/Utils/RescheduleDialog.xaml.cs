using System;
using System.Windows;

using UroborosApp.Model;

namespace UroborosApp.Utils
{
    public partial class RescheduleDialog : Window
    {
        private readonly int _materialId;

        public DateTime Tomorrow => DateTime.Today.AddDays(1);

        public RescheduleDialog(int materialId)
        {
            InitializeComponent();
            _materialId = materialId;
            DatePicker.SelectedDate = Tomorrow;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = Entities.GetContext();
                var newRepetition = new Repetition
                {
                    material_id = _materialId,
                    repetition_date = DatePicker.SelectedDate.Value.Date + TimePicker.Time,
                    recall_score = 0
                };

                db.Repetition.Add(newRepetition);
                db.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}