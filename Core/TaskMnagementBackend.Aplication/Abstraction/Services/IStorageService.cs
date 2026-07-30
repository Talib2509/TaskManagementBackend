using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface IStorageService
    {
      
        Task<(string FilePath, string? ThumbnailPath)> UploadFileAsync(IFormFile file, string subFolder = "tasks");
        Task DeleteFileAsync(string fileName, string subFolder = "tasks");
        string GetFileUrl(string fileName, string subFolder = "tasks");
       
        Task<string> AppendChunkAsync(IFormFile chunk, string uniqueFileName, string subFolder = "tasks");
    }
}