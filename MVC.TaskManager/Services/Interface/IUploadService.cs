namespace MVC.TaskManager.Services.Interface
{
    public interface IUploadService
    {
        Task <string>UploadPhoto(IFormFile file);
    }
}
