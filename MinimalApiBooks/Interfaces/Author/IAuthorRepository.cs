
using MinimalApiBooks.DTOs.Pagination;

namespace MinimalApiBooks.Interfaces.Author
{
	public interface IAuthorRepository
	{
		Task<List<Entities.Author>> GetAll(PaginationDTO paginationDTO);
		Task<Entities.Author?> GetById(int id);
		Task<List<Entities.Author>> GetByName(string name);
		Task<bool> Exist(int id);
		Task<int> Create(Entities.Author author);
		Task Delete(int id);
		Task Update(Entities.Author author);
	}
}
