using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Azure.Storage.Blobs;
using Azure.Messaging.EventGrid;
using Moq;
using System;
using DocumentUploader.Specs.Auth;
using Microsoft.AspNetCore.Authentication;
using TechTalk.SpecFlow.Assist;

namespace DocumentUploader.Specs
{
    public class DocumentUploaderApiFactory : WebApplicationFactory<DocumentUploader.API.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace BlobServiceClient with a mock
                services.RemoveAll(typeof(BlobServiceClient));
                var blobMock = new Mock<BlobServiceClient>();
                services.AddSingleton(blobMock.Object);

                // Replace EventGridPublisherClient with a mock
                services.RemoveAll(typeof(EventGridPublisherClient));
                var eventGridMock = new Mock<EventGridPublisherClient>();
                services.AddSingleton(eventGridMock.Object);

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, FakeJwtBearerHandler>("Test", options => { });

                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });

            });
        }
    }
}

