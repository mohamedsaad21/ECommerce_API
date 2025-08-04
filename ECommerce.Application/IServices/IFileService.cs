using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.IServices
{
    public interface IFileService
    {
        List<string> GetByUrls(int EntityId, string wwwRootPath, string folderName);
        Task UploadAsync(int EntityId, List<IFormFile> files, string wwwRootPath, string folderName);
        void DeleteAsync(int EntityId, string wwwRootPath, string folderName);
    }
}
