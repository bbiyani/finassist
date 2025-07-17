using System.Threading.Tasks;

namespace DocumentUploader.API.Services.Interfaces
{
    public interface IEventPublisherService
    {
        Task PublishDocumentUploadedAsync(string documentUrl, string uploadedBy);
    }
}