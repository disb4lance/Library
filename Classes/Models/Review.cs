using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Внешний ключ для пользователя
        public User User { get; set; } // Навигационное свойство
        public int BookId { get; set; } // Внешний ключ для книги
        public Book Book { get; set; } // Навигационное свойство
        public string Comment { get; set; }
        public int Rating { get; set; } // Рейтинг от 1 до 5
        public DateTime CreatedAt { get; set; }
    }
}
