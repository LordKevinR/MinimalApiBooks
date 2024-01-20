using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MinimalApiBooks.Interfaces.FileStorage;

namespace MinimalApiBooks.Services.Azure
{
	public class AzureFileStorage : IFileStorage
	{
		private readonly string connectionString;

		public AzureFileStorage(IConfiguration configuration)
        {
			connectionString = configuration.GetConnectionString("AzureStorage")!;
        }
		public async Task<string> Store(string container, IFormFile file)
		{
			var client = new BlobContainerClient(connectionString, container);
			await client.CreateIfNotExistsAsync();
			client.SetAccessPolicy(PublicAccessType.Blob);
			 
			var extension = Path.GetExtension(file.FileName);
			var fileName = $"{Guid.NewGuid()}{extension}";
			var blob = client.GetBlobClient(fileName);
			var blobHttpHeaders = new BlobHttpHeaders();
			blobHttpHeaders.ContentType = file.ContentType;
			await blob.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
			return blob.Uri.ToString();
		}

        public async Task Delete(string? path, string container)
		{
			if (string.IsNullOrEmpty(path))
				return;

			var client = new BlobContainerClient(connectionString, container);
			await client.CreateIfNotExistsAsync();
			var fileName = Path.GetFileName(path);
			var blob = client.GetBlobClient(fileName);
			await blob.DeleteIfExistsAsync();
		}

	}
}
