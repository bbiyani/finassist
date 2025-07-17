using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace DocumentUploader.Specs.StepDefinitions
{
    [Binding]
    public class UploadDocumentIntegrationSteps
    {
        private readonly HttpClient _client;
        private HttpResponseMessage _response;

        public UploadDocumentIntegrationSteps()
        {
            var factory = new DocumentUploaderApiFactory();
            _client = factory.CreateClient();
        }

        [When(@"I POST a PDF file to the upload API")]
        public async Task WhenIPOSTAPDFFileToTheUploadAPI()
        {
            var content = new MultipartFormDataContent();
            var pdfContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("dummy pdf content"));
            pdfContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            content.Add(pdfContent, "file", "sample.pdf");
            content.Add(new StringContent("tester@finassist.com"), "uploadedBy");

            _response = await _client.PostAsync("/api/upload", content);
        }

        [Then(@"the API should respond with 200 OK")]
        public void ThenTheAPIShouldRespondWith200OK()
        {
            Assert.True(_response.IsSuccessStatusCode, $"Expected 200 OK but got {_response.StatusCode}");
        }
    }
}
