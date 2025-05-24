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
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private Entities _context;

        public ChangePasswordWindow()
        {
            InitializeComponent();
            _context = new Entities();
        }

        private void SavePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OldPasswordBox.Password) ||
                string.IsNullOrEmpty(NewPasswordBox.Password) ||
                string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Новые пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _context.users.Find(CurrentUser.Id);
            if (user.password_hash != MainWindow.GetHash(OldPasswordBox.Password)) 
            {
                MessageBox.Show("Неверный текущий пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            user.password_hash = MainWindow.GetHash(NewPasswordBox.Password);
            _context.SaveChanges();

            MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
