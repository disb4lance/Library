using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
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
    /// Логика взаимодействия для Loan.xaml
    /// </summary>
    public partial class Loan : Window
    {
        private string _token;
        public Loan(string token)
        {
            _token = token;
            InitializeComponent();
            ImportLoan();
        }

        private void ImportLoan()
        {
            // Извлечение UserId из токена
            string userId = GetUserIdFromToken(_token);
            if (!string.IsNullOrEmpty(userId))
            {
                SendUserIdToServer(userId);
            }
            else
            {
                MessageBox.Show("Ошибка извлечения UserId из токена.");
            }
        }

        private string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Найти клейм с именем "id"
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "id");

            return userIdClaim?.Value;
        }

        private async void SendUserIdToServer(string userId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5062/api/Loans/GetLoansByUserId?userId={userId}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Данные получены успешно: {responseData}");
                }
                else
                {
                    MessageBox.Show("Ошибка при получении данных.");
                }
            }
        }
    }
}
