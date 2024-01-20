namespace MinimalApiBooks.DTOs.Comment
{
	public class CommentResponseDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public DateTime CreatedDateTime { get; set; }
		public int Likes { get; set; } = 0;
		public int BookId { get; set; }
	}
}
