using Microsoft.Extensions.Hosting;

using System.Threading;
using System.Threading.Tasks;

namespace CampaignKit.WorldMap.Tests.MockServices
{
    public class MockTileCreationService : BackgroundService
    {
        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Otherwise it's running
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
