using Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            User user = new User
            {
                UserName = LoginUsernameTextBox.Text,
                PasswordHash = LoginPasswordBox.Password
            };
            LogIn(user);

        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            

        }
        private async Task LogIn(User user)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Установите базовый адрес вашего API
                    client.BaseAddress = new Uri("http://localhost:5062/api/");


                    string json = JsonSerializer.Serialize(user);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Отправка POST-запроса
                    HttpResponseMessage response = await client.PostAsync("Account/LogIn", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Вход успешен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Очистка полей после успешного входа
                        LoginUsernameTextBox.Clear();
                        LoginPasswordBox.Clear();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Ошибка входа: {error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string HashPassword(string password)
        {
            // Используем SHA256
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Преобразуем строку в байты с использованием UTF8
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                // Получаем хэш в виде байтового массива
                byte[] hash = sha256.ComputeHash(bytes);

                // Строим строку из хэша
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
