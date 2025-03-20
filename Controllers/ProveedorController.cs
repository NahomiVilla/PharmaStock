using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PharmaStock.Models;
using PharmaStock.Repositories;
using PharmaStock.Services;
using PharmaStockAI.Models;
using System;
using System.Threading.Tasks;


namespace PharmaStock.Controllers
{
    [ApiController]
    [Route("api/proveedor")]
    public class ProveedorController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProveedorController> _logger;
        private readonly EmailService _emailService;
        private readonly ProveedoresRepository _proveedoresRepository;
        public ProveedorController(ProductRepository productRepository, ILogger<ProveedorController> logger,EmailService emailService,ProveedoresRepository proveedoresRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
            _emailService=emailService;
            _proveedoresRepository=proveedoresRepository;

        }
        
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] Proveedores model)
        {
            var prov = _proveedoresRepository.GetByEmail(model.Email);

            if (prov != null)
                return NotFound(new { message = "Proveedor ya registrado" });

            
            var newProv = _proveedoresRepository.Registrar(model);
            return Ok(newProv);
        }

        [HttpPost("reponer")]
        public async Task<IActionResult> ReponerProducto([FromBody] ReposicionRequest request)
        {
            try
            {
                var producto = await _productRepository.GetByIdAsync(request.ProductoId);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                producto.CantidadActual += request.Cantidad;
                await _productRepository.UpdateAsync(producto);

                _logger.LogInformation($"✅ Producto {producto.Nombre} repuesto con {request.Cantidad} unidades.");

                // Enviar notificación por correo
                string emailAdmin = "admin@pharmastock.com"; // Puedes obtenerlo de la BD si es necesario
                string asunto = "📢 Reposición de Producto";
                string mensaje = $"Se han agregado {request.Cantidad} unidades de {producto.Nombre}. Nuevo stock: {producto.CantidadActual}.";

                _emailService.EnviarCorreo(emailAdmin, asunto, mensaje);


                return Ok(new { mensaje = $"Stock actualizado: {producto.Nombre} ahora tiene {producto.CantidadActual} unidades." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en la reposición de producto.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }

}
