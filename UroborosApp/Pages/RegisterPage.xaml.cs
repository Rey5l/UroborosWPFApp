using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Data.Entity;
using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        public RegistrationResult RegisterUser(string username, string email, string password, string repeatPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return new RegistrationResult(false, "Все поля должны быть заполнены");

            if (username.Length < 5)
                return new RegistrationResult(false, "Длина имени должна быть больше 5 символов");

            if (username.Contains(" "))
                return new RegistrationResult(false, "Имя пользователя не должно содержать пробелы");

            if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
                return new RegistrationResult(false, "Email введен некорректно");

            if (password.Length < 8)
                return new RegistrationResult(false, "Длина пароля должна быть больше 8 символов");

            if (password != repeatPassword)
                return new RegistrationResult(false, "Пароли не совпадают");

            try
            {
                using (var db = new UroborosDBEntities())
                {
                    if (db.users.Any(u => u.email == email))
                        return new RegistrationResult(false, "Пользователь с таким email уже существует");

                    if (db.users.Any(u => u.name == username))
                        return new RegistrationResult(false, "Пользователь с таким логином уже существует");

                    var newUser = new users
                    {
                        name = username,
                        email = email,
                        password_hash = MainWindow.GetHash(password),
                        registration_date = DateTime.Now,
                        avatar_url = "/avatars/default.png",
                        last_login = DateTime.Now,
                        role = "user",
                        is_active = true
                    };

                    db.users.Add(newUser);
                    db.SaveChanges();

                    return new RegistrationResult(true, "Вы успешно зарегистрировались!");
                }
            }
            catch (Exception ex)
            {
                return new RegistrationResult(false, $"Ошибка регистрации: {ex.Message}");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var result = RegisterUser(
                RegisterUsername.Text.Trim(),
                RegisterEmail.Text.Trim(),
                RegisterPassword.Password.Trim(),
                RepeatingPassword.Password.Trim());

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new LoginPage());
            }
            else
            {
                MessageBox.Show(result.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SwitchToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }

    public class RegistrationResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public RegistrationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}