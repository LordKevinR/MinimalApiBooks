namespace MinimalApiBooks.Entities
{
	public class Book
	{
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime PublicatitionDate { get; set; }
        public int NumberOfPages { get; set; }
        public string? CoverImage { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<GenreBook> GenresBooks { get; set; } = new List<GenreBook>();
    }
}
