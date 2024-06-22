using Azure.Storage.Blobs;

namespace reservation_system_be.Services.FileServices
{
    public class FileService: IFileService
    {
        private string _containerName;
        private readonly BlobServiceClient _blobServiceClient;

        public FileService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<string> Upload(IFormFile file, string containerName)
        {
            try
            {
                _containerName = containerName;

                // Generate a UUID
                string uuid = Guid.NewGuid().ToString();

                // Construct new filename with UUID
                string newFileName = $"{uuid}";

                // Create container instance
                var containerInstance = _blobServiceClient.GetBlobContainerClient(_containerName);

                // Create blob instance
                string fileExtension = Path.GetExtension(file.FileName);

                string blobName = $"{newFileName}{fileExtension}";

                var blobInstance = containerInstance.GetBlobClient(blobName);

                // File save in storage
                await blobInstance.UploadAsync(file.OpenReadStream());

                // Return the generated new filename
                return blobName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred uploading file: {ex.Message}");
                throw;
            }
        }
        public async Task<Stream> Get(string name, string containerName)
        {
            _containerName = containerName;
            //create container instance
            var containerInstance = _blobServiceClient.GetBlobContainerClient(_containerName);

            //create blob instance
            var blobInstance = containerInstance.GetBlobClient(name);

            var downloadContent = await blobInstance.DownloadAsync();

            return downloadContent.Value.Content;

        }
        public async Task Delete(string name, string containerName)
        {
            _containerName = containerName;
            //create container instance
            var containerInstance = _blobServiceClient.GetBlobContainerClient(_containerName);

            //create blob instance
            var blobInstance = containerInstance.GetBlobClient(name);
            //delete blob
            await blobInstance.DeleteAsync();

        }

    }
}
