using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using DocumentUploader.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using SharedLibs.Models;

namespace DocumentUploader.API.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobStorageSettings _settings;

        public BlobStorageService(BlobServiceClient blobServiceClient, IOptions<BlobStorageSettings> options)
        {
            _blobServiceClient = blobServiceClient;
            _settings = options.Value;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream);

            return blobClient.Uri.ToString();
        }
    }
}