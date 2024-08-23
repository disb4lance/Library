using Classes.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using Client.Services.Requests;

namespace Client
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string mail = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            IUserFactory userFactory;

            if (username.ToLower().Contains("admin"))
            {
                userFactory = new AdminUserFactory();
            }
            else
            {
                userFactory = new RegularUserFactory();
            }


            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаем UserService и создаем пользователя
            var userService = new UserService(userFactory);
            User newUser = userService.RegisterUser(username, password, mail);

            await RegisterUserAsync(newUser);
        }

        public async Task RegisterUserAsync(User user)
        {
            var apiClient = new ApiClient();


            var response = await apiClient.PostUserAsync("Account/register", user);

            if (response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                UsernameTextBox.Clear();
                EmailTextBox.Clear();
                PasswordBox.Clear();
                ConfirmPasswordBox.Clear();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
