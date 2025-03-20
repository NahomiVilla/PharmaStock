using System.Linq;
using PharmaStock.Data;
using PharmaStock.Models;
using PharmaStockAI.Models;

namespace PharmaStock.Repositories
{
    public class ProveedoresRepository
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Proveedores? GetByEmail(string email)
        {
            return _context.Proveedores.FirstOrDefault(p => p.Email == email);
        }

        public Proveedores Registrar(Proveedores proveedor)
        {
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
            return proveedor;
        }
    } 
}