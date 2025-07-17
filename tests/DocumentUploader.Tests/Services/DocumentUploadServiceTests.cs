using Xunit;
using Moq;
using System.Threading.Tasks;
using System;
using System.IO;
using DocumentUploader.API.Services;
using DocumentUploader.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using SharedLibs.Models;

namespace DocumentUploader.Tests.Services
{
    public class DocumentUploadServiceTests
    {
        private readonly Mock<IBlobStorageService> _blobMock;
        private readonly Mock<IEventPublisherService> _eventMock;
        private readonly DocumentUploadService _sut;

        public DocumentUploadServiceTests()
        {
            _blobMock = new Mock<IBlobStorageService>();
            _eventMock = new Mock<IEventPublisherService>();
            _sut = new DocumentUploadService(_blobMock.Object, _eventMock.Object);
        }

        [Fact]
        public async Task HandleUploadAsync_ShouldUploadAndPublish_WhenValidRequest()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.pdf";
            fileMock.Setup(f => f.FileName).Returns(fileName);

            var uploadedBy = "user@finassist.com";
            var blobUrl = "https://storage.blob.core.windows.net/documents/test.pdf";

            var request = new UploadRequest
            {
                File = fileMock.Object,
                UploadedBy = uploadedBy
            };

            _blobMock.Setup(x => x.UploadAsync(fileMock.Object)).ReturnsAsync(blobUrl);

            // Act
            var result = await _sut.HandleUploadAsync(request);

            // Assert
            Assert.Equal(blobUrl, result);
            _blobMock.Verify(x => x.UploadAsync(fileMock.Object), Times.Once);
            _eventMock.Verify(x => x.PublishDocumentUploadedAsync(blobUrl, uploadedBy), Times.Once);
        }

        [Fact]
        public async Task HandleUploadAsync_ShouldThrow_WhenBlobUploadFails()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("fail.pdf");

            var request = new UploadRequest
            {
                File = fileMock.Object,
                UploadedBy = "fail@finassist.com"
            };

            _blobMock.Setup(x => x.UploadAsync(fileMock.Object))
                     .ThrowsAsync(new Exception("Upload failed"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _sut.HandleUploadAsync(request));
            Assert.Equal("Upload failed", ex.Message);

            _eventMock.Verify(x => x.PublishDocumentUploadedAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
