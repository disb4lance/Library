using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.LoginModel
{
    public class Loan
    {
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
    }
}
