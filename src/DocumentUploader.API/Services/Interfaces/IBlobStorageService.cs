using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentUploader.API.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
