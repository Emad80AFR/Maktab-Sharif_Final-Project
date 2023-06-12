using Microsoft.AspNetCore.Http;

namespace FrameWork.Application.FileUpload
{
    public interface IFileUploader
    {
        Task<string> Upload(IFormFile? file, string path, CancellationToken cancellationToken);
    }
}
