
using System.ComponentModel.DataAnnotations;    
using System.ComponentModel.DataAnnotations.Schema;

using Proveedores = PharmaStockAI.Models.Proveedores;

namespace PharmaStock.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        public long Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        public required string Categoria { get; set; }
        [Required]
        public int CantidadActual { get; set; }
        [Required]
        public int CantidadMinima { get; set; }=10;
        [Required]
        public decimal Precio { get; set; }
        public required DateTime FechaVencimiento { get; set; }
        [Required]
        [ForeignKey("Proveedores")]
        public required long? ProveedorId { get; set; }
        public  Proveedores? Proveedor { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;//DATETIME.utcnow GUARDA LA FECHA DE CREACION AUTOMATICAMENTE
    }
}