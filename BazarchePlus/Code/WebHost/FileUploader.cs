﻿using FrameWork.Application;
using FrameWork.Application.FileUpload;

namespace WebHost
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> Upload(IFormFile? file, string path, CancellationToken cancellationToken)
        {
            if (file == null) return "";

            var directoryPath = $"{_webHostEnvironment.WebRootPath}//UploadedPictures//{path}";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
            var filePath = $"{directoryPath}//{fileName}";
            await using var output = File.Create(filePath);
            await file.CopyToAsync(output, cancellationToken);
            return $"{path}/{fileName}";
        }
    }
}
