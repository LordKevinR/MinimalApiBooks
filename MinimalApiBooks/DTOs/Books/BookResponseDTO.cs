using MinimalApiBooks.DTOs.Comment;

namespace MinimalApiBooks.DTOs.Books
{
	public class BookResponseDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string ISBN { get; set; } = null!;
		public DateTime PublicatitionDate { get; set; }
		public int NumberOfPages { get; set; }
		public string? CoverImage { get; set; } = null!;
		public string Description { get; set; } = null!;
        public List<CommentResponseDTO> Comments { get; set; } = new List<CommentResponseDTO>();
    }
}
