using Microsoft.EntityFrameworkCore;
using MinimalApiBooks.Interfaces.Genre;

namespace MinimalApiBooks.Repositories.Genre
{
	public class GenreRepository : IGenreRepository
	{
		private readonly ApplicationDbContext context;

		public GenreRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
		public async Task<List<Entities.Genre>> GetAll()
		{
			return await context.Genres.OrderBy(x => x.Name).ToListAsync();
		}

		public async Task<Entities.Genre?> GetById(int id)
		{
			return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<bool> Exist(int id)
		{
			return await context.Genres.AnyAsync(x => x.Id == id);
		}

		public async Task<List<int>> ExistList(List<int> ids)
		{
			return await context.Genres.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();
		}

		public async Task<int> Create(Entities.Genre genres)
		{
			context.Add(genres);
			await context.SaveChangesAsync();
			return genres.Id;
		}

		public async Task Update(Entities.Genre genre)
		{
			context.Update(genre);
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Genres.Where(x => x.Id == id).ExecuteDeleteAsync();
		}
	}
}
