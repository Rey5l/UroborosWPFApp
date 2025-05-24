using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

using Microsoft.Win32;

using UroborosApp.Model;
using UroborosApp.Utils;

namespace UroborosApp.Pages
{
    public partial class ProfilePage : Page
    {
        private Entities _context;

        public ProfilePage()
        {
            InitializeComponent();
            _context = Entities.GetContext();
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                var user = _context.users.Find(CurrentUser.Id);

                if (user != null)
                {
                    NameTextBox.Text = user.name;
                    EmailTextBox.Text = user.email;
                    RegistrationDateText.Text = user.registration_date?.ToString("dd.MM.yyyy") ?? "Не указана";
                    LastLoginText.Text = user.last_login?.ToString("dd.MM.yyyy HH:mm") ?? "Не указан";

                    CurrentUser.Username = user.name;
                    CurrentUser.Email = user.email;

                    if (!string.IsNullOrEmpty(user.avatar_url))
                    {
                        AvatarImageBrush.ImageSource = new BitmapImage(new Uri(user.avatar_url));
                    }
                    else
                    {
                        AvatarImageBrush.ImageSource = new BitmapImage(
                            new Uri("C:\\Users\\user\\source\\repos\\UroborosApp\\UroborosApp\\avatars\\default.png", UriKind.Relative));
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден в базе данных");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg)|*.png;*.jpg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var user = _context.users.Find(CurrentUser.Id);
                    if (user != null)
                    {
                        user.avatar_url = openFileDialog.FileName;
                        _context.SaveChanges();

                        AvatarImageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                        MessageBox.Show("Аватар успешно обновлен!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _context.users.Find(CurrentUser.Id);
                if (user != null)
                {
                    user.name = NameTextBox.Text;
                    _context.SaveChanges();

                    CurrentUser.Username = NameTextBox.Text;

                    MessageBox.Show("Данные успешно сохранены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?",
                                      "Подтверждение выхода",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CurrentUser.Id = 0;
                CurrentUser.Username = null;
                CurrentUser.Email = null;

                NavigationService.Navigate(new LoginPage());
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var passwordWindow = new ChangePasswordWindow();
            passwordWindow.ShowDialog();
        }


        private void NavigateToHomePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }

        private void NavigateToCategoriesPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoriesPage());
        }

        private void NavigateToReminderPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReminderPage());
        }

        private void NavigateToReviewSchedulePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReviewSchedulePage());
        }

        private void NavigateToProgressPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProgressPage());
        }

        private void NavigateToStatistics(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StatisticsPage());
        }

        private void NavigateToTasksPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TasksPage());
        }

        private void NavigateToActivityLog(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ActivityLogPage());
        }

        private void NavigateToLearningGoalsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LearningGoalsPage());
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("my_soderzhanie.chm");
        }
    }
}