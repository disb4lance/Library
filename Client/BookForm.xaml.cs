using Classes.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Client
{
    public partial class BookForm : Window
    {
        private readonly bool _isEditMode;
        private readonly int _bookId;
        private string _token;

        public BookForm(string token)
        {
            InitializeComponent();
            _isEditMode = false;
            _token = token;
        }

        public BookForm(int bookId, string title, string author, string isbn, DateTime publishedDate, string genres)
        {
            InitializeComponent();

            _isEditMode = true;
            _bookId = bookId;

            TitleTextBox.Text = title;
            AuthorTextBox.Text = author;
            ISBNTextBox.Text = isbn;
            PublishedDatePicker.SelectedDate = publishedDate;
            GenresTextBox.Text = genres;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем значения из текстовых полей
                string title = TitleTextBox.Text;
                string author = AuthorTextBox.Text;
                string isbn = ISBNTextBox.Text;
                DateTime? publishedDate = PublishedDatePicker.SelectedDate;
                var genresText = GenresTextBox.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // Преобразуем строки в объекты Genre
                ICollection<Genre> genres = genresText.Select(g => new Genre { Name = g.Trim() }).ToList();

                // Создаем новый объект Book
                Book newBook = new Book
                {
                    Title = title,
                    Author = author,
                    ISBN = isbn,
                    PublishedDate = publishedDate ?? DateTime.Now,
                    Genres = genres
                };

                SaveBook(newBook);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книги: {ex.Message}");
            }
        }

        private async void SaveBook(Book book) {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    // Установите базовый адрес вашего API
                    client.BaseAddress = new Uri("http://localhost:5062/api/");


                    string json = JsonSerializer.Serialize(book);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Отправка POST-запроса
                    HttpResponseMessage response = await client.PostAsync("Books/AddBook", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Книга успешно сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        TitleTextBox.Clear();
                        AuthorTextBox.Clear();
                        ISBNTextBox.Clear();
                        GenresTextBox.Clear();

                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Ошибка сохранения: {error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
