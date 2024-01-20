using Microsoft.EntityFrameworkCore;
using MinimalApiBooks.Entities;

namespace MinimalApiBooks
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Genre>().Property(g => g.Name).HasMaxLength(50);

			modelBuilder.Entity<Author>().Property(p => p.Name).HasMaxLength(150);
			modelBuilder.Entity<Author>().Property(p => p.Photo).IsUnicode();

			modelBuilder.Entity<Book>().Property(b => b.Title).HasMaxLength(250);
			modelBuilder.Entity<Book>().Property(p => p.CoverImage).IsUnicode();

			modelBuilder.Entity<Comment>().Property(p => p.Title).HasMaxLength(250);

			modelBuilder.Entity<GenreBook>().HasKey(gb => new { gb.BookId, gb.GenreId });
		}

		public DbSet<Genre> Genres { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GenreBook> GenresBooks { get; set; }
    }
}
