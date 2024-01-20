namespace MinimalApiBooks.Entities
{
	public class Author
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string? Photo { get; set; }
    }
}
