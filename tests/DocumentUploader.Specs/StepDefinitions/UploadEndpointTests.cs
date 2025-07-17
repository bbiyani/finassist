using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace DocumentUploader.Specs.Integration
{
    public class UploadEndpointTests
    {
        [Fact]
        public async Task Upload_Endpoint_Should_AllowAuthenticatedRequest()
        {
            var factory = new DocumentUploaderApiFactory();
            var client = factory.CreateClient();

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("tester@fake.com"), "uploadedBy");
            content.Add(new ByteArrayContent(new byte[1]), "file", "test.pdf");

            var response = await client.PostAsync("/api/upload", content);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
