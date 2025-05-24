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

namespace UroborosApp.Utils
{
    /// <summary>
    /// Логика взаимодействия для RecallScoreDialog.xaml
    /// </summary>
    public partial class RecallScoreDialog : Window
    {
        public int SelectedScore { get; private set; } = 3;

        public RecallScoreDialog()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            RadioButton selected = Score1.IsChecked == true ? Score1 :
                                  Score2.IsChecked == true ? Score2 :
                                  Score3.IsChecked == true ? Score3 :
                                  Score4.IsChecked == true ? Score4 :
                                  Score5.IsChecked == true ? Score5 : null;

            if (selected != null && int.TryParse(selected.Tag.ToString(), out int score))
            {
                SelectedScore = score;
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
            Close();
        }
    }
}
