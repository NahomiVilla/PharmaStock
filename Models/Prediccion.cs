

namespace PharmaStock.Models
{
    public class Prediccion
    {
        public required Producto Producto { get; set; }
        public int Ventas_Predichas { get; set; }
        public required string Fecha_Prediccion { get; set; }
    }
}    
