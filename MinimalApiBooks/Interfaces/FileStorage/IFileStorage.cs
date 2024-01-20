namespace MinimalApiBooks.Interfaces.FileStorage
{
	public interface IFileStorage
	{
		Task Delete(string? path, string container);
		Task<string> Store(string container, IFormFile file);
		async Task<string> Update(string? path, string container, IFormFile file)
		{
			await Delete(path, container);
			return await Store(container, file);
		}
	}
}
