using MinimalApiBooks.Interfaces.Genre;

namespace MinimalApiBooks.Services.Genre
{
	public class GenreService : IGenreService
	{
		private readonly IGenreRepository genreRepository;

		public GenreService(IGenreRepository genreRepository)
		{
			this.genreRepository = genreRepository;
		}

		public async Task<List<Entities.Genre>> GetAll()
		{
			return await genreRepository.GetAll();
		}

		public async Task<Entities.Genre?> GetById(int id)
		{
			return await genreRepository.GetById(id);
		}
		public async Task<bool> Exist(int id)
		{
			return await genreRepository.Exist(id);
		}

		public async Task<int> Create(Entities.Genre genres)
		{
			return await genreRepository.Create(genres);
		}


		public async Task Update(Entities.Genre genre)
		{
			await genreRepository.Update(genre);
		}

		public async Task Delete(int id)
		{
			await genreRepository.Delete(id);
		}

		public async Task<List<int>> ExistList(List<int> ids)
		{
			return await genreRepository.ExistList(ids);
		}
	}
}
