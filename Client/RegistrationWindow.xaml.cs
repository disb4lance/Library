using Classes.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Проверка введенных данных
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Введите имя пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                MessageBox.Show("Введите действительный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = new User()
            {
                name = UsernameTextBox.Text,
                email = EmailTextBox.Text,
                password = PasswordBox.Password,
            };

            // Отправка данных пользователя на сервер
            await RegisterUserAsync(user);

        }

        private async Task RegisterUserAsync(User user)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Установите базовый адрес вашего API
                    client.BaseAddress = new Uri("http://localhost:5062/api/");

                    // Преобразуйте объект пользователя в JSON
                    string json = JsonSerializer.Serialize(user);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Отправка POST-запроса
                    
                    HttpResponseMessage response = await client.PostAsync("Account/Register", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Регистрация успешна!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Очистка полей после успешной регистрации
                        UsernameTextBox.Clear();
                        EmailTextBox.Clear();
                        PasswordBox.Clear();
                        ConfirmPasswordBox.Clear();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Ошибка регистрации: {error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Вспомогательный метод для проверки адреса электронной почты
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
