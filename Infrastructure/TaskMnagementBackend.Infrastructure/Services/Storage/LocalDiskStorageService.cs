using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Infrastructure.Services.Storage
{
    public class LocalDiskStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;

        private readonly Dictionary<string, List<byte[]>> _allowedMagicBytes = new()
        {
            { ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } },
            { ".webp", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } },
            { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },
            { ".docx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } },
            { ".xlsx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } },
            { ".mp4", new List<byte[]> {
                new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70 },
                new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70 },
                new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70 }
            } },
            { ".mov", new List<byte[]> { new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70 } } },
            { ".txt", new List<byte[]> { Array.Empty<byte>() } }
        };

        public LocalDiskStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<(string FilePath, string? ThumbnailPath)> UploadFileAsync(IFormFile file, string subFolder = "tasks")
        {
            if (file == null || file.Length == 0)
                throw new Exception("Fayl boş ola bilməz.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedMagicBytes.ContainsKey(extension) || !await IsValidFileSignature(file, extension))
            {
                throw new Exception("Fayl formatı dəstəklənmir və ya saxta fayldır!");
            }

            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", subFolder);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string? thumbnailPath = null;

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp")
            {
                string thumbFileName = $"thumb_{uniqueFileName}";
                string thumbFullPath = Path.Combine(uploadPath, thumbFileName);

                file.OpenReadStream().Position = 0;

                using var image = await Image.LoadAsync(file.OpenReadStream());
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(200, 200),
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsync(thumbFullPath);
                thumbnailPath = Path.Combine("uploads", subFolder, thumbFileName).Replace("\\", "/");
            }

            string originalPath = Path.Combine("uploads", subFolder, uniqueFileName).Replace("\\", "/");
            return (originalPath, thumbnailPath);
        }

        
        public async Task<string> AppendChunkAsync(IFormFile chunk, string uniqueFileName, string subFolder = "tasks")
        {
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", subFolder);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string filePath = Path.Combine(uploadPath, uniqueFileName);

           
            using (var stream = new FileStream(filePath, FileMode.Append))
            {
                await chunk.CopyToAsync(stream);
            }

            return Path.Combine("uploads", subFolder, uniqueFileName).Replace("\\", "/");
        }

        public Task DeleteFileAsync(string fileName, string subFolder = "tasks")
        {
            string filePath = Path.Combine(_env.WebRootPath, fileName);
            if (File.Exists(filePath)) File.Delete(filePath);
            return Task.CompletedTask;
        }

        public string GetFileUrl(string fileName, string subFolder = "tasks")
        {
            return $"/{fileName}";
        }

        private async Task<bool> IsValidFileSignature(IFormFile file, string extension)
        {
            var signatures = _allowedMagicBytes[extension];

            if (signatures.Any(s => s.Length == 0)) return true;

            using var reader = new BinaryReader(file.OpenReadStream());
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            bool isMatch = signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));

            file.OpenReadStream().Position = 0;
            return isMatch;
        }
    }
}