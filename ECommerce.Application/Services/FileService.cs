using ECommerce.Application.IServices;
using Microsoft.AspNetCore.Http;


namespace ECommerce.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<string> GetByUrls(int EntityId, string wwwRootPath, string folderName)
        {
            var httpContext = _httpContextAccessor.HttpContext; // Get HttpContext

            var EntityPath = Path.Combine(wwwRootPath, @"images\" + folderName + "\\" + EntityId);
            List<string> ImageUrls = new List<string>();
            if (Directory.Exists(EntityPath))
            {
                var EntityFiles = Directory.GetFiles(EntityPath);
                foreach (var File in EntityFiles)
                {
                    var ImageUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{folderName}/{EntityId}/{Path.GetFileName(File)}";

                    //var ImageUrl = Path.Combine(@"\images\" + folderName + "\\" + EntityId + "\\", Path.GetFileName(File));
                    ImageUrls.Add(ImageUrl);
                }
            }
            return ImageUrls;
        }

        public async Task UploadAsync(int EntityId, List<IFormFile> files, string wwwRootPath, string folderName)
        {

            if (files != null)
            {
                if (!Directory.Exists(Path.Combine(wwwRootPath, @"images\" + folderName + "\\" + EntityId)))
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(wwwRootPath, @"images\" + folderName + "\\" + EntityId));
                }
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string EntityPath = Path.Combine(wwwRootPath, @"images\" + folderName + "\\" + EntityId);
                    using (var fileStream = new FileStream(Path.Combine(EntityPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
        }

        public void DeleteAsync(int EntityId, string wwwRootPath, string folderName)
        {

            if (Directory.Exists(Path.Combine(wwwRootPath, @"images\" + folderName + "\\" + EntityId.ToString())))
            {
                System.IO.Directory.Delete(Path.Combine(wwwRootPath, @"images\" + folderName + "\\" + EntityId.ToString()), true);
            }
        }
    }
}
