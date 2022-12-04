using LibraryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Username = "librarian",
                    Password = "123",
                    IsAdmin= true,
                },
                new User
                {
                    Username = "jeremy",
                    Password = "123",
                    IsAdmin = false,
                },
                new User
                {
                    Username = "james",
                    Password = "123",
                    IsAdmin = false,
                });

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Author = "Jeremy Clarkson",
                    Title = "Can You Make This Thing Go Faster",
                    Date = 2020,
                    Publisher = "Penguin Random House UK"
                },
                new Book
                {
                    Id = 2,
                    Author = "Jeremy Clarkson",
                    Title = "Diddly Squat - a Year on the Farm",
                    Date = 2020,
                    Publisher = "Penguin Random House UK"
                },
                new Book
                {
                    Id = 3,
                    Author = "F. Scott Fitzgerald",
                    Title = "The Great Gatsby",
                    Date = 1925,
                    Publisher = "Charles Scribner\u0027s Sons"
                });
        }
    }
}
