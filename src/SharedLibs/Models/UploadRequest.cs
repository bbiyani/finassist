using Microsoft.AspNetCore.Http;

namespace SharedLibs.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public string UploadedBy { get; set; }
    }
}
