using Microsoft.EntityFrameworkCore;
using PharmaStock.Models;
using PharmaStockAI.Models;

namespace PharmaStock.Data
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
       
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }

        public DbSet<Users> Usuarios { get; set; }        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany()
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Producto>().Property(p => p.Precio).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Producto>().Property(p => p.CantidadMinima).HasDefaultValue(10);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PharmaStockDB;Username=postgres;Password=S251094v");
        }
    }
}