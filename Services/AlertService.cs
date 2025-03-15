
using PharmaStock.Repositories;


namespace PharmaStock.Services
{
    public class AlertService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AlertService> _logger;
        private readonly TimeSpan _intervalo = TimeSpan.FromHours(1); // Verifica cada hora

        public AlertService(IServiceProvider serviceProvider, ILogger<AlertService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var productRepository = scope.ServiceProvider.GetRequiredService<ProductRepository>();
                        var ProveedorService = scope.ServiceProvider.GetRequiredService<ProveedorService>();
                        
                        var productos = productRepository.GetAll(); 

                        var fechaLimite = DateTime.UtcNow.AddDays(14);
                        var productosCriticos = productos.Where(p => p.CantidadActual <= p.CantidadMinima || 
                                                                     (p.FechaVencimiento != default && p.FechaVencimiento <= fechaLimite));

                        if (productosCriticos.Any())
                        {
                            _logger.LogWarning("⚠️ Productos con stock bajo o próximos a vencer detectados.");
                            foreach (var producto in productosCriticos)
                            {
                                _logger.LogWarning($"🔴 {producto.Nombre} - Stock: {producto.CantidadActual} - Vence: {producto.FechaVencimiento}");
                                if (producto.CantidadActual <= producto.CantidadMinima)
                                {
                                    _logger.LogWarning($"🚚 Ordenando reposición para {producto.Nombre}");
                                    await ProveedorService.OrdenarReposicion(producto);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en la verificación de alertas.");
                }

                await Task.Delay(_intervalo, stoppingToken); // Espera el tiempo definido antes de la siguiente verificación
            }
        }
    }
}
