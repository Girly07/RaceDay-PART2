
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDay.API.Services
{
    public class BlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobService(IConfiguration configuration)
        {
            // These will come from appsettings.json
            _connectionString = configuration["AzureBlobStorage:ConnectionString"] ?? "";
            _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "racedayimages";
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // If no connection string is provided, return a mock URL for testing
            if (string.IsNullOrEmpty(_connectionString) || _connectionString == "YOUR_AZURE_CONNECTION_STRING")
            {
                return $"https://racedaymockstorage.blob.core.windows.net/{_containerName}/{Guid.NewGuid()}_{file.FileName}";
            }

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Create container if it doesn't exist
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = blobContainerClient.GetBlobClient($"{Guid.NewGuid()}_{file.FileName}");

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            return blobClient.Uri.ToString();
        }
    }
}