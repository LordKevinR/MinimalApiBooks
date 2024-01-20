namespace MinimalApiBooks.Interfaces.Genre
{
	public interface IGenreRepository
	{
		Task<List<Entities.Genre>> GetAll();
		Task<Entities.Genre?> GetById(int id);
		Task<int> Create(Entities.Genre genres);
		Task<bool> Exist(int id);
		Task Update(Entities.Genre genre);
		Task Delete(int id);
		Task<List<int>> ExistList(List<int> ids);
	}
}
