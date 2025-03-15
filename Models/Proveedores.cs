using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PharmaStock.Models;

namespace PharmaStockAI.Models
{
    public class Proveedores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public required string Nombre { get; set; }

        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public string? Direccion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Relación con productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
