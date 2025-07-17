using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using DocumentUploader.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using SharedLibs.Models;

namespace DocumentUploader.API.Services
{
    public class EventPublisherService : IEventPublisherService
    {
        private readonly EventGridPublisherClient _eventGridClient;
        private readonly EventGridSettings _settings;

        public EventPublisherService(EventGridPublisherClient eventGridClient, IOptions<EventGridSettings> options)
        {
            _eventGridClient = eventGridClient;
            _settings = options.Value;
        }

        public async Task PublishDocumentUploadedAsync(string documentUrl, string uploadedBy)
        {

            var eventGridEvent = new EventGridEvent(
                subject: $"document/{Guid.NewGuid()}",
                eventType: "document.uploaded",
                dataVersion: "1.0",
                data: new
                {
                    documentUrl = documentUrl,
                    uploadedBy = uploadedBy,
                    uploadedAt = DateTime.UtcNow
                });

            await _eventGridClient.SendEventAsync(eventGridEvent);
        }
    }
}