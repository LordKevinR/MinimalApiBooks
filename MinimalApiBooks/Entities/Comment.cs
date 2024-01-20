namespace MinimalApiBooks.Entities
{
	public class Comment
	{
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public int Likes { get; set; }
        public int BookId { get; set; }
    }
}
