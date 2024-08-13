using Classes.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Client
{
    public partial class BookForm : Window
    {
        private readonly bool _isEditMode;
        private readonly int _bookId;

        public BookForm()
        {
            InitializeComponent();
            _isEditMode = false;  
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

                // Здесь можно сохранить объект newBook в базу данных через API или другой механизм
                MessageBox.Show("Книга успешно сохранена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книги: {ex.Message}");
            }
        }
    }
    
}
