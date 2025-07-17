using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibs.Logging;
using SharedLibs.Auth;
using SharedLibs.Models;
using Azure.Storage.Blobs;
using Azure.Messaging.EventGrid;
using DocumentUploader.API.Services;
using DocumentUploader.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using DocumentUploader.API.Auth;

namespace DocumentUploader.API
{
    public class Startup
    {
        public readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            // SharedLibs setup
            services.AddAppLogging(_config);
            if (_env.IsDevelopment())
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, FakeJwtBearerHandler>("Test", _ => { });
            }
            else
            {
                services.AddAzureAdAuth(_config);
            }


            // Config bindings
            services.Configure<BlobStorageSettings>(_config.GetSection("BlobStorage"));
            services.Configure<EventGridSettings>(_config.GetSection("EventGrid"));

            // Azure SDK clients
            services.AddSingleton(x => new BlobServiceClient(_config["AzureStorage:ConnectionString"]));
            services.AddSingleton(x => new EventGridPublisherClient(
                new Uri(_config["EventGrid:Endpoint"]),
                new Azure.AzureKeyCredential(_config["EventGrid:Key"])));

            // Custom Services
            services.AddScoped<IDocumentUploadService, DocumentUploadService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IEventPublisherService, EventPublisherService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}