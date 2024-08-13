using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Внешний ключ для пользователя
        public User User { get; set; } // Навигационное свойство
        public int BookId { get; set; } // Внешний ключ для книги
        public Book Book { get; set; } // Навигационное свойство
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
