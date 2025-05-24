
using System;
using System.Windows;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Utils
{
    public partial class LearningGoalsDialog : Window
    {
        public Learning_Goals LearningGoal { get; set; }

        public LearningGoalsDialog(Learning_Goals goal = null)
        {
            InitializeComponent();

            LearningGoal = goal ?? new Learning_Goals
            {
                User_id = CurrentUser.Id,
                Title = string.Empty,
                Description = string.Empty,
                Target_Date = null,
                Is_completed = false,
                Created_Date = DateTime.Now
            };

            DataContext = LearningGoal;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Название не может быть пустым.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TitleTextBox.Text.Length > 255)
            {
                MessageBox.Show("Название не может превышать 255 символов.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(DescriptionTextBox.Text) && DescriptionTextBox.Text.Length > 255)
            {
                MessageBox.Show("Описание не может превышать 255 символов.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TargetDatePicker.SelectedDate.HasValue && TargetDatePicker.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Срок выполнения не может быть в прошлом.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LearningGoal.Title = TitleTextBox.Text;
            LearningGoal.Description = DescriptionTextBox.Text;
            LearningGoal.Target_Date = TargetDatePicker.SelectedDate;

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
