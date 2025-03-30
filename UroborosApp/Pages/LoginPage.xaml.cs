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
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Auth(LoginUsername.Text.Trim(), LoginPassword.Password.Trim());
        }

        private void SwitchToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        public bool Auth(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (login.Contains(" ") || password.Contains(" "))
            {
                MessageBox.Show("Логин и пароль не должны содержать пробелы", "Ошибка",
                      MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            password = MainWindow.GetHash(password);

            using (var db = new UroborosDBEntities())
            {
                var currentUser = db.users.FirstOrDefault(u => u.name == login && u.password_hash == password);

                if (currentUser != null)
                {
                    CurrentUser.Id = currentUser.id;
                    CurrentUser.Username = currentUser.name;
                    CurrentUser.Email = currentUser.email;
                    //NavigationService.Navigate(new HomePage());
                    return true;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
        }
    }
}
