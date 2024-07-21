using Classes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.DataBase
{
    public class datacontext : DbContext
    {
        public datacontext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
