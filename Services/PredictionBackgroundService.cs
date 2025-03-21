using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace PharmaStock.Services
{
    public class PredictionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PredictionBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var predictionService = scope.ServiceProvider.GetRequiredService<PredictionService>();

                    await predictionService.FetchAndBroadcastPrediction();
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken); // Ejecuta cada 5 minutos
            }
        }
    }
}
