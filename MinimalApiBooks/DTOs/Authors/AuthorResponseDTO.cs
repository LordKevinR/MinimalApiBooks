namespace MinimalApiBooks.DTOs.Authors
{
	public class AuthorResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public DateTime BirthDate { get; set; }
		public string? Photo { get; set; }
	}
}
