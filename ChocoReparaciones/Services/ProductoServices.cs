using ChocoReparaciones.Data;
using ChocoReparaciones.Models;
using Microsoft.EntityFrameworkCore;

namespace ChocoReparaciones.Services;

public class ProductoServices
{
	private readonly ApplicationDbContext _context;

	public ProductoServices(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<Producto>> ObtenerTodos()
	{
		return await _context.Productos
			.OrderByDescending(p => p.Id)
			.ToListAsync();
	}
	public async Task<List<Producto>> ObtenerProductos()
	{
		return await _context.Productos
			.Where(p => p.CantidadDisponible > 0)
			.OrderByDescending(p => p.Id)
			.ToListAsync();
	}
	public async Task AgregarProducto(Producto producto)
	{
		_context.Productos.Add(producto);
		await _context.SaveChangesAsync();
	}

	public async Task<Producto?> ObtenerPorId(int id)
	{
		return await _context.Productos.FindAsync(id);
	}

	public async Task EliminarProducto(int id)
	{
		var producto = await _context.Productos.FindAsync(id);
		if (producto != null)
		{
			_context.Productos.Remove(producto);
			await _context.SaveChangesAsync();
		}
	}

	public async Task ActualizarProducto(Producto producto)
	{
		_context.Productos.Update(producto);
		await _context.SaveChangesAsync();
	}
}

