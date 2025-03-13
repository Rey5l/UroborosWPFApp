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
using System.Windows.Navigation;
using System.Windows.Shapes;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginUsername.Text.Trim();
            string password = MainWindow.GetHash(LoginPassword.Password.Trim());

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new UroborosDBEntities())
            {
                var currentUser = db.users.FirstOrDefault(u => u.name == login && u.password_hash == password);

                if (currentUser != null)
                {
                    CurrentUser.Id = currentUser.id;
                    CurrentUser.Username = currentUser.name;
                    CurrentUser.Email = currentUser.email;
                    NavigationService.Navigate(new HomePage());
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

        private void SwitchToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
