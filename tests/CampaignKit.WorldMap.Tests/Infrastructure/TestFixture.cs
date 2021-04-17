using CampaignKit.WorldMap.Services;
using CampaignKit.WorldMap.Tests.MockServices;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using CampaignKit.WorldMap.Core;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class TestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Replace blob storage service with mock service
                var blobServiceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IBlobStorageService));
                services.Remove(blobServiceDescriptor);
                services.AddSingleton<IBlobStorageService, MockBlobStorageService>();

                // Replace table storage service with mock service
                var tableStorageDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITableStorageService));
                services.Remove(tableStorageDescriptor);
                services.AddSingleton<ITableStorageService, MockTableStorageService>();

                // Replace tile creation service with mock service
                var tileServiceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IHostedService) && descriptor.ImplementationType == typeof(TileCreationService));
                services.Remove(tileServiceDescriptor);
                services.AddSingleton<IHostedService, MockTileCreationService>();

                // Add authentication options
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        "Test Scheme"; // has to match scheme in TestAuthenticationExtensions
                    options.DefaultChallengeScheme = "Test Scheme";
                }).AddTestAuth(o => { });
            });
        }
    }
}
