using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace userVoice.Services
{
    public class InAppStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InAppStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(String fileRoute, String containerName)
        {
            var fileName = Path.GetFileName(fileRoute);
            String fileDirectory = Path.Combine(env.WebRootPath, containerName, fileName);
            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }

            return Task.FromResult(0);
        }

        public async Task<String> EditFile(byte[] content, String extension, String containerName, String fileRoute, String contentType)
        {
            if (!String.IsNullOrEmpty(fileRoute))
            {
                await DeleteFile(fileRoute, containerName);
            }

            return await SaveFile(content, extension, containerName, contentType);
        }

        public async Task<String> SaveFile(byte[] content, String extension, String containerName, String contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            String folder = Path.Combine(env.WebRootPath, containerName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            String savingPath = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(savingPath, content);

            var currentUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}:\\{httpContextAccessor.HttpContext.Request.Host}";
            var pathForDatabase = Path.Combine(currentUrl, containerName, fileName).Replace("\\", "/");
            return pathForDatabase;
        }
    }
}
