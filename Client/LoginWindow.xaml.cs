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
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string name = LoginUsernameTextBox.Text;
            string password = HashPassword(LoginPasswordBox.Password);
            LogIn(name, password);

        }
        private async Task LogIn(string name, string password)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Установите базовый адрес вашего API
                    client.BaseAddress = new Uri("http://localhost:5062/api/");

                    // Кодирование параметров запроса для безопасной передачи в URL
                    string query = $"Account/LogIn?name={Uri.EscapeDataString(name)}&password={Uri.EscapeDataString(password)}";

                    // Отправка GET-запроса
                    HttpResponseMessage response = await client.GetAsync(query);

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
            // Используем SHA256 вместо MD5 для большей безопасности
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
