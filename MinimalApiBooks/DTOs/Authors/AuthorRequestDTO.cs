namespace MinimalApiBooks.DTOs.Authors
{
	public class AuthorRequestDTO
	{
		public string Name { get; set; } = null!;
		public DateTime BirthDate { get; set; }
		public IFormFile? Photo { get; set; }
	}
}
