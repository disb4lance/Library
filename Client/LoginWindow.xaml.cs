using Classes.LoginModel;
using Classes.Models;
using Client.Services.Requests;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            LoginModel loginModel = new LoginModel
            {
                UserName = LoginUsernameTextBox.Text,
                Password = LoginPasswordBox.Password
            };
            LogIn(loginModel);

        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            

        }
        private async Task LogIn(LoginModel user)
        {
            var apiClient = new ApiClient();
            var response = await apiClient.LoginAsync("Account/LogIn", user);

            if (response.IsSuccess)
            {
                MessageBox.Show("Вход успешен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Пример передачи токена в следующую форму
                BooksListWindow booksListWindow = new BooksListWindow();
                booksListWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(response.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
