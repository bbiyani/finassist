using TechTalk.SpecFlow;
using Xunit;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharedLibs.Models;
using DocumentUploader.API.Services;
using DocumentUploader.API.Services.Interfaces;
//no infrastructure needed
//Testing pure logic and behavior
namespace DocumentUploader.Specs.StepDefinitions
{
    [Binding]
    public class UploadDocumentServiceSteps
    {
        private UploadRequest _request;
        private string _resultUrl;
        private DocumentUploadService _service;

        private readonly Mock<IBlobStorageService> _blobMock = new();
        private readonly Mock<IEventPublisherService> _eventMock = new();

        [Given(@"a user has a valid document")]
        public void GivenAUserHasAValidDocument()
        {
            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns("invoice.pdf");

            _request = new UploadRequest
            {
                File = fileMock.Object,
                UploadedBy = "user@test.com"
            };

            _blobMock.Setup(x => x.UploadAsync(_request.File))
                     .ReturnsAsync("https://blob/invoice.pdf");

            _service = new DocumentUploadService(_blobMock.Object, _eventMock.Object);
        }

        [When(@"the service processes the upload")]
        public async Task WhenTheServiceProcessesTheUpload()
        {
            _resultUrl = await _service.HandleUploadAsync(_request);
        }

        [Then(@"the file URL should be returned")]
        public void ThenTheFileURLShouldBeReturned()
        {
            Assert.Equal("https://blob/invoice.pdf", _resultUrl);
        }

        [Then(@"an event should be published")]
        public void ThenAnEventShouldBePublished()
        {
            _eventMock.Verify(e => e.PublishDocumentUploadedAsync("https://blob/invoice.pdf", "user@test.com"), Times.Once);
        }
    }
}
