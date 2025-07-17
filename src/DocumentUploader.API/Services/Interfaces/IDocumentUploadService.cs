using System.Threading.Tasks;
using SharedLibs.Models;

namespace DocumentUploader.API.Services.Interfaces
{
    public interface IDocumentUploadService
    {
        Task<string> HandleUploadAsync(UploadRequest request);
    }
}
