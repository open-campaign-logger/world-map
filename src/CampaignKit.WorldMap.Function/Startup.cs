using CampaignKit.WorldMap.Core.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CampaignKit.WorldMap.TileProcessor.Startup))]

namespace CampaignKit.WorldMap.TileProcessor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add the blob storage service to the context.
            builder.Services.AddSingleton<IBlobStorageService, DefaultBlobStorageService>();
            builder.Services.AddSingleton<ITableStorageService, DefaultTableStorageService>();
            builder.Services.AddSingleton<IQueueStorageService, DefaultQueueStorageService>();
            builder.Services.AddSingleton<IMapProcessingService, DefaultMapProcessingService>();

        }
    }
}
