using Classes.Models;
using Client.Services.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
    /// Логика взаимодействия для addToLoan.xaml
    /// </summary>
    public partial class addToLoan : Window
    {
        private string _token;
        public addToLoan(string token)
        {
            InitializeComponent();
            _token = token;
        }

        private async void OnAddToCartClick(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string bookTitle = BookTitleTextBox.Text;
                string encodedBookTitle = Uri.EscapeDataString(bookTitle);

                // Укажите URL вашего API
                string url = $"http://localhost:5062/api/Books/GetBookByName?name={encodedBookTitle}";

                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();


                string responseData = await response.Content.ReadAsStringAsync();
                Book book = JsonConvert.DeserializeObject<Book>(responseData);


                // Извлечение UserId из JWT токена
                string userId = GetUserIdFromToken(_token);

                // Подготовьте данные для создания записи о займе
                var loan = new
                {
                    UserId = userId,
                    BookId = book.Id,
                    LoanDate = DateTime.Now
                };

                // Отправьте POST запрос на сервер для создания записи о займе
                string loanUrl = "http://localhost:5062/api/Loans/AddToLoan";
                string json = JsonConvert.SerializeObject(loan);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage loanResponse = await client.PostAsync(loanUrl, content);

                if (loanResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Книга успешно добавлена в корзину.");
                }
                else
                {
                    MessageBox.Show("Не удалось добавить книгу в корзину.");
                }
            }
        }

        private string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            return userId;
        }
    }
}
