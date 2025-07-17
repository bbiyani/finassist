using DocumentUploader.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibs.Models;
using System.Threading.Tasks;

namespace DocumentUploader.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IDocumentUploadService _uploadService;

        public UploadController(IDocumentUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadRequest request)
        {
            var result = await _uploadService.HandleUploadAsync(request);
            return Ok(result);
        }
    }
}
