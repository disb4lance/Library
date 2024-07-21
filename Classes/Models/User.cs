using Classes.Intarfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Models
{
    public class User : IUser
    {

        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }


    }
}
