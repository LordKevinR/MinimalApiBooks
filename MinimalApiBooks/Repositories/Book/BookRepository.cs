using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalApiBooks.DTOs.Pagination;
using MinimalApiBooks.Entities;
using MinimalApiBooks.Interfaces.Book;
using MinimalApiBooks.Utilities.ExtensionsMethods;

namespace MinimalApiBooks.Repositories.Book
{
	public class BookRepository : IBookRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;
		private readonly HttpContext httpContext;

		public BookRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
		{
			this.context = context;
			this.mapper = mapper;
			httpContext = httpContextAccessor.HttpContext!;
		}

		public async Task<List<Entities.Book>> GetAll(PaginationDTO paginationDTO)
		{
			var queryable = context.Books.Include(c => c.Comments).AsQueryable();
			await httpContext.InsertPaginationParametersInHeader(queryable);
			return await queryable.OrderBy(x => x.Title).Paginate(paginationDTO).ToListAsync();
		}

		public async Task<Entities.Book?> GetById(int id)
		{
			return await context.Books.Include(c => c.Comments).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<Entities.Book>> GetByTitle(string title)
		{
			return await context.Books.Where(a => a.Title.Contains(title)).OrderBy(a => a.Title).ToListAsync();
		}

		public Task<bool> Exist(int id)
		{
			return context.Books.AnyAsync(x => x.Id == id);
		}

		public async Task<int> Create(Entities.Book book)
		{
			context.Add(book);
			await context.SaveChangesAsync();
			return book.Id;
		}

		public async Task Update(Entities.Book book)
		{
			context.Update(book);
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Books.Where(x => x.Id == id).ExecuteDeleteAsync();
		}

		public async Task AssignGenres(int id, List<int> genresIds)
		{
			var book = await context.Books.Include(b => b.GenresBooks).FirstOrDefaultAsync(b => b.Id == id);

			if(book is null)
			{
				throw new ArgumentException($"There's not any book with the Id: {id}");
			}

			var genresBooks = genresIds.Select(genreId => new GenreBook() { GenreId = genreId });

			book.GenresBooks = mapper.Map(genresBooks, book.GenresBooks);

			await context.SaveChangesAsync();
		}
	}
}
