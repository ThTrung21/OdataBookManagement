using Microsoft.EntityFrameworkCore;
using static ODataBookStore.Models.EDM;

namespace ODataBookStore
{
	public class BookStoreContext: DbContext
	{
		public BookStoreContext() { }
		public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }
		public DbSet<Book> Books { get; set; }
		public DbSet<Press> Presses { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Book>().OwnsOne(c => c.Location);
		}
	}
}
