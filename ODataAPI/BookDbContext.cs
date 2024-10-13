using Microsoft.EntityFrameworkCore;
using ODataAPI.Models;

namespace ODataAPI
{
	public class BookDbContext: DbContext
	{
		public DbSet<EDM.Book> Books { get; set; }
		public DbSet<EDM.Press> Presses { get; set; }
		  
		public BookDbContext(DbContextOptions<BookDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EDM.Book>()
		.OwnsOne(b => b.Location);  // Treat Address as a complex type

			// Seed Press data
			modelBuilder.Entity<EDM.Press>().HasData(
				new EDM.Press { Id = 1, Name = "TechPress", Category = EDM.Category.Book }
			);

			// Seed Book data without setting Location as a navigation property
			modelBuilder.Entity<EDM.Book>().HasData(
				new EDM.Book
				{
					Id = 1,
					ISBN = "1234567890",
					Title = "Learning ASP.NET",
					Author = "John Doe",
					Price = 29.99M,
					// Do not assign Location here
				}
			);

			// Seed Address data separately
			modelBuilder.Entity<EDM.Book>()
				.OwnsOne(b => b.Location)
				.HasData(
					new { BookId = 1, City = "New York", Street = "5th Avenue" } // Directly specify address properties
				);
			modelBuilder.Entity<EDM.Book>()
				 .HasOne(b => b.Press)
				 .WithMany() // Adjust as needed
				 .HasForeignKey(b => b.PressId);
		}
	}
}
