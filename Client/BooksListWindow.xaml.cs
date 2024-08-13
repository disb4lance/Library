using Classes.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public partial class BooksListWindow : Window
    {
        public BooksListWindow()
        {
            InitializeComponent();
            LoadBooksAsync();
        }

        private async void LoadBooksAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/");

                HttpResponseMessage response = await client.GetAsync("Books");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var books = JsonSerializer.Deserialize<List<Book>>(jsonString);
                    BooksDataGrid.ItemsSource = books;
                }
                else
                {
                    MessageBox.Show("Failed to load books.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchTextBox.Text;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/");

                HttpResponseMessage response = await client.GetAsync($"Books/search?title={query}&author={query}&genre={query}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var books = JsonSerializer.Deserialize<List<Book>>(jsonString);
                    BooksDataGrid.ItemsSource = books;
                }
                else
                {
                    MessageBox.Show("Failed to search books.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var bookForm = new BookForm();
            bookForm.ShowDialog();
            LoadBooksAsync();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int bookId = (int)((Button)sender).Tag;
            var book = (Book)BooksDataGrid.SelectedItem;

            var bookForm = new BookForm(bookId, book.Title, book.Author, book.ISBN, book.PublishedDate, string.Join(",", book.Genres));
            bookForm.ShowDialog();
            LoadBooksAsync();
        }
    }
}
