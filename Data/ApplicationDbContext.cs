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
            optionsBuilder.UseNpgsql("Host=dpg-cvarb8qn91rc739a0lm0-a.oregon-postgres.render.com;Port=5432;Database=pharmastock;Username=pharmastock_user;Password=3v6OCkb7dowRnOGsKc7pQ8vgp2IsrSdq;SSL Mode=Require;Trust Server Certificate=true;");
        }
    }
}