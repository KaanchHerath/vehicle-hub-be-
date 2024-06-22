namespace reservation_system_be.Services.FileServices
{
    public interface IFileService
    {
        Task<string> Upload(IFormFile file, string containerName);
        Task<Stream> Get(string name, string containerName);
        Task Delete(string name, string containerName);
    }
}
