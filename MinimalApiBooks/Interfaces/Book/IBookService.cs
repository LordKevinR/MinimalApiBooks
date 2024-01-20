using MinimalApiBooks.DTOs.Pagination;

namespace MinimalApiBooks.Interfaces.Book
{
	public interface IBookService
	{
		Task<int> Create(Entities.Book book);
		Task Delete(int id);
		Task<bool> Exist(int id);
		Task<List<Entities.Book>> GetAll(PaginationDTO paginationDTO);
		Task<Entities.Book?> GetById(int id);
		Task<List<Entities.Book>> GetByTitle(string title);
		Task Update(Entities.Book book);
		Task AssignGenres(int id, List<int> genresIds);

	}
}
