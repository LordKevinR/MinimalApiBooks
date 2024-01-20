namespace MinimalApiBooks.Interfaces.Comment
{
	public interface ICommentService
	{
		Task<int> Create(Entities.Comment comment);
		Task Delete(int id);
		Task<bool> Exist(int id);
		Task<List<Entities.Comment>> GetAll(int bookId);
		Task<Entities.Comment?> GetById(int id);
		Task Update(Entities.Comment comment);
	}
}
