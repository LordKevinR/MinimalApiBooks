using MinimalApiBooks.DTOs.Pagination;
using MinimalApiBooks.Interfaces.Book;

namespace MinimalApiBooks.Services.Book
{
	public class BookService : IBookService
	{
		private readonly IBookRepository bookRepository;

		public BookService(IBookRepository bookRepository)
        {
			this.bookRepository = bookRepository;
		}

		public async Task AssignGenres(int id, List<int> genresIds)
		{
			await bookRepository.AssignGenres(id, genresIds);
		}

		public async Task<int> Create(Entities.Book book)
		{
			return await bookRepository.Create(book);
		}

		public async Task Delete(int id)
		{
			await bookRepository.Delete(id);
		}

		public Task<bool> Exist(int id)
		{
			return bookRepository.Exist(id);
		}

		public Task<List<Entities.Book>> GetAll(PaginationDTO paginationDTO)
		{
			return bookRepository.GetAll(paginationDTO);
		}

		public Task<Entities.Book?> GetById(int id)
		{
			return bookRepository.GetById(id);
		}

		public Task<List<Entities.Book>> GetByTitle(string title)
		{
			return bookRepository.GetByTitle(title);
		}

		public async Task Update(Entities.Book book)
		{
			await bookRepository.Update(book);
		}
	}
}
