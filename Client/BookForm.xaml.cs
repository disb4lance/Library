using Classes.Models;
using Client.Services.Requests;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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


        private async void AddBookToLoan() { 

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Создаем новый объект Book
                Book newBook = new Book
                {
                    Title = TitleTextBox.Text,
                    Author = AuthorTextBox.Text,
                    ISBN = ISBNTextBox.Text,
                    PublishedDate = PublishedDatePicker.SelectedDate ?? DateTime.Now,
                    Genres = GenresTextBox.Text.Split(new[] { ',' },                                           // получаем все жанры
                    StringSplitOptions.RemoveEmptyEntries).Select(g => new Genre { Name = g.Trim() }).ToList()
                };

                SaveBook(newBook);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книги: {ex.Message}");
            }
        }

        private async void SaveBook(Book book)
        {
            var apiClient = new ApiClient(_token);

            var response = await apiClient.SaveBookAsync(book);

            if (response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(response.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
