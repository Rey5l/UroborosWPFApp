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

namespace UroborosApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterUsername.Text.Length > 0)
            {
                string name = RegisterUsername.Text.Trim();
                string email = RegisterEmail.Text.Trim();
                string password = MainWindow.GetHash(RegisterPassword.Password.Trim());
                string repeatPassword = MainWindow.GetHash(RepeatingPassword.Password.Trim());

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (name.Length < 5)
                {
                    MessageBox.Show("Длина имени должна быть больше 5 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
                {
                    MessageBox.Show("Email введен некорректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (password.Length < 5)
                {
                    MessageBox.Show("Длина пароля должна быть больше 5 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (password != repeatPassword)
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var uroborosDB = new UroborosDBEntities())
                {
                    var existingUserEmail = uroborosDB.users.FirstOrDefault(u => u.email == email);
                    if (existingUserEmail != null)
                    {
                        MessageBox.Show("Пользователь с таким email уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var existingUserLogin = uroborosDB.users.FirstOrDefault(u => u.name == name);
                    if (existingUserLogin != null)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                
                UroborosDBEntities db = new UroborosDBEntities();
                users userObject = new users
                {
                    name = name,
                    email = email,
                    password_hash = password,
                    registration_date = DateTime.Now
                };

                db.users.Add(userObject);
                db.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались!", "Успешно!", MessageBoxButton.OK);
                NavigationService.Navigate(new LoginPage());
                

            }
        }

        private void SwitchToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
