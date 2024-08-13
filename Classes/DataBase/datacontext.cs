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
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Genre> Genres { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка TPH (Table-per-Hierarchy) — все пользователи будут храниться в одной таблице
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<AdminUser>("Admin")
                .HasValue<RegularUser>("Regular");

            base.OnModelCreating(modelBuilder);
            // многие ко многим
            modelBuilder.Entity<Genre>()
               .HasMany(g => g.Books)
               .WithMany(b => b.Genres)
               .UsingEntity(j => j.ToTable("BookGenres"));

            // Связь один ко многим между User и Reviews
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // Связь один ко многим между User и Loans
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId);
        }
    }
}
