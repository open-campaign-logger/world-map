﻿using CampaignKit.WorldMap.Tests.MockServices;
using CampaignKit.WorldMap.Core.Services;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using CampaignKit.WorldMap.UI;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{
    public class TestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.UseContentRoot("");

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
