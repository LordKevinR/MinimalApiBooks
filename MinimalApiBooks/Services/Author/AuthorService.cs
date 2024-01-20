using MinimalApiBooks.DTOs.Pagination;
using MinimalApiBooks.Interfaces.Author;

namespace MinimalApiBooks.Services.Author
{
	public class AuthorService : IAuthorService
	{
		private readonly IAuthorRepository authorRepository;

		public AuthorService(IAuthorRepository authorRepository)
        {
			this.authorRepository = authorRepository;
		}
        public async Task<int> Create(Entities.Author author)
		{
			return await authorRepository.Create(author);
		}

		public async Task Delete(int id)
		{
			await authorRepository.Delete(id);
		}

		public async Task<bool> Exist(int id)
		{
			return await authorRepository.Exist(id);
		}

		public async Task<List<Entities.Author>> GetAll(PaginationDTO paginationDTO)
		{
			return await authorRepository.GetAll(paginationDTO);
		}

		public async Task<Entities.Author?> GetById(int id)
		{
			return await authorRepository.GetById(id);
		}

		public async Task<List<Entities.Author>> GetByName(string name)
		{
			return await authorRepository.GetByName(name);
		}

		public async Task Update(Entities.Author author)
		{
			await authorRepository.Update(author);
		}
	}
}
