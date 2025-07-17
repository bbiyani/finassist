using System.Threading.Tasks;
using SharedLibs.Models;
using DocumentUploader.API.Services.Interfaces;

namespace DocumentUploader.API.Services
{
    public class DocumentUploadService : IDocumentUploadService
    {
        private readonly IBlobStorageService _blobStorage;
        private readonly IEventPublisherService _eventPublisher;

        public DocumentUploadService(IBlobStorageService blobStorage, IEventPublisherService eventPublisher)
        {
            _blobStorage = blobStorage;
            _eventPublisher = eventPublisher;
        }

        public async Task<string> HandleUploadAsync(UploadRequest request)
        {
            var blobUrl = await _blobStorage.UploadAsync(request.File);
            await _eventPublisher.PublishDocumentUploadedAsync(blobUrl, request.UploadedBy);
            return blobUrl;
        }
    }
}