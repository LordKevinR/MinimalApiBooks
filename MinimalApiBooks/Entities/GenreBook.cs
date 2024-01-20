namespace MinimalApiBooks.Entities
{
	public class GenreBook
	{
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
