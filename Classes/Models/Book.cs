using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Classes.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        // Связь многие ко многим с жанрами
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}
