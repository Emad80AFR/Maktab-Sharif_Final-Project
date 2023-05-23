using Microsoft.AspNetCore.Http;

namespace FrameWork.Application
{
    public interface IFileUploader
    {
        Task<string> Upload(IFormFile? file, string path);
    }
}
