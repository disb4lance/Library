﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.LoginModel
{
    public class LoanDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public BookDto Book { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
