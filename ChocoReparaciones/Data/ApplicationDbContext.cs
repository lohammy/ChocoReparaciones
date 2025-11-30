using ChocoReparaciones.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChocoReparaciones.Data;

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
	public DbSet<Cliente> Clientes { get; set; }
	public DbSet<Producto> Productos { get; set; }
	public DbSet<Reparacion> Reparaciones { get; set; }
	public DbSet<Usuario> Usuarios { get; set; }
}
