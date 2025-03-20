
using PharmaStock.Models;
using PharmaStock.Data;
using Microsoft.EntityFrameworkCore;

namespace PharmaStock.Repositories{
    public class ProductRepository{
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context){
            _context = context;
        }
        public List<Producto> GetAll(){
            return _context.Productos.ToList();
        }
        public async Task<Producto> GetByIdAsync(long id){
            return (await _context.Productos.FirstOrDefaultAsync(p => p.Id == id))!;
        }
        public Producto Agregar(Producto producto){
            if (producto.CantidadMinima>=0)
            {
                producto.CantidadMinima = 0;
            }
            producto.FechaVencimiento = DateTime.SpecifyKind(producto.FechaVencimiento, DateTimeKind.Utc);
            _context.Productos.Add(producto);
            _context.SaveChanges();
            return producto;
        }
        public Producto Update(long id,Producto producto){
            
            var Producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            
            if(Producto != null){
                Producto.Nombre = producto.Nombre;
                Producto.Descripcion = producto.Descripcion;
                Producto.Categoria = producto.Categoria;
                Producto.CantidadActual = producto.CantidadActual;
                Producto.CantidadMinima = producto.CantidadMinima;
                Producto.Precio = producto.Precio;
                Producto.FechaVencimiento = producto.FechaVencimiento;
            }
            _context.SaveChanges();
            return producto;
        }
        public async Task<Producto> UpdateAsync(Producto producto){
            var Producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == producto.Id);
            if(Producto != null){
                Producto.Nombre = producto.Nombre;
                Producto.Descripcion = producto.Descripcion;
                Producto.Categoria = producto.Categoria;
                Producto.CantidadActual = producto.CantidadActual;
                Producto.CantidadMinima = producto.CantidadMinima;
                Producto.Precio = producto.Precio;
                Producto.FechaVencimiento = producto.FechaVencimiento;
            }
            await _context.SaveChangesAsync();
            return producto;
        }
        public void Delete(Producto producto){
            _context.Productos.Remove(producto);
            _context.SaveChanges();
        }
    }
}