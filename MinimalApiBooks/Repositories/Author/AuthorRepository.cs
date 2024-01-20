using Microsoft.EntityFrameworkCore;
using MinimalApiBooks.DTOs.Pagination;
using MinimalApiBooks.Interfaces.Author;
using MinimalApiBooks.Utilities.ExtensionsMethods;

namespace MinimalApiBooks.Repositories.Author
{
	public class AuthorRepository : IAuthorRepository
	{
		private readonly ApplicationDbContext context;
		private readonly HttpContext httpContext;

		public AuthorRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			this.context = context;
			httpContext = httpContextAccessor.HttpContext!;
		}

		public async Task<List<Entities.Author>> GetAll(PaginationDTO paginationDTO)
		{
			var queryable = context.Authors.AsQueryable();
			await httpContext.InsertPaginationParametersInHeader(queryable);
			return await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
		}

		public async Task<Entities.Author?> GetById(int id)
		{
			return await context.Authors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<Entities.Author>> GetByName(string name)
		{
			return await context.Authors.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();
		}

		public Task<bool> Exist(int id)
		{
			return context.Authors.AnyAsync(x => x.Id == id);
		}

		public async Task<int> Create(Entities.Author author)
		{
			context.Add(author);
			await context.SaveChangesAsync();
			return author.Id;
		}

		public async Task Update(Entities.Author author)
		{
			context.Update(author);
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Authors.Where(x => x.Id == id).ExecuteDeleteAsync();
		}
	}
}
