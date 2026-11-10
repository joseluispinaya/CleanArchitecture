namespace CapaWeb.Helpers
{
    public interface IFileStorage
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
