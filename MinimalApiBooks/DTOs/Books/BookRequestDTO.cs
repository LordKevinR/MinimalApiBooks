namespace MinimalApiBooks.DTOs.Books
{
	public class BookRequestDTO
	{
		public string Title { get; set; } = null!;
		public string ISBN { get; set; } = null!;
		public DateTime PublicatitionDate { get; set; }
		public int NumberOfPages { get; set; }
		public IFormFile? CoverImage { get; set; } = null!;
		public string Description { get; set; } = null!;
	}
}
