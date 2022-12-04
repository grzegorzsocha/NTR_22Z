using LibraryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
