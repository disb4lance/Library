using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Classes.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
