using BookStoreUtilities;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(b => b.RowVersion).IsConcurrencyToken();
        }

        public virtual DbSet<Book> Book { get; set; }
    }
}
