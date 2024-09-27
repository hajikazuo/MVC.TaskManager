using MVC.TaskManager.Data;
using MVC.TaskManager.Services.Interface;
using System.Drawing.Text;

namespace MVC.TaskManager.Services.Implementation
{
    public class UploadService : IUploadService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly string _filePath;

        public UploadService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
            _filePath = Path.Combine(env.WebRootPath, _config.GetValue<string>("FolderUpload"));
        }

        public async Task<string> UploadPhoto(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_filePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }

            return null;
        }
    }
}
