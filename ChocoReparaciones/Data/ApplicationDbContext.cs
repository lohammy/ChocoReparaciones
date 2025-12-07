using ChocoReparaciones.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChocoReparaciones.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
	public DbSet<Producto> Productos { get; set; }
	public DbSet<Reparacion> Reparaciones { get; set; }
	public DbSet<ReparacionDetalle> ReparacionDetalles { get; set; } 
	public DbSet<Usuario> Usuarios { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<ReparacionDetalle>()
			.HasOne(d => d.Reparacion)
			.WithMany(r => r.ReparacionDetalles)
			.HasForeignKey(d => d.ReparacionId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ReparacionDetalle>()
			.HasOne(d => d.Producto)
			.WithMany(p => p.ReparacionDetalles)
			.HasForeignKey(d => d.ProductoId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}