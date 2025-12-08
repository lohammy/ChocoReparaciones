using ChocoReparaciones.Data;
using ChocoReparaciones.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChocoReparaciones.Services;

public class ProductoServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
	public async Task<bool> Guardar(Producto producto)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		if (!await Existe(producto.Id))
		{
			return await Insertar(producto);
		}
		else
		{
			return await Modificar(producto);
		}
	}

	private async Task<bool> Existe(int productoId)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		return await _context.Productos
			.AnyAsync(p => p.Id == productoId);
	}

	private async Task<bool> Insertar(Producto producto)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		_context.Productos.Add(producto);
		return await _context.SaveChangesAsync() > 0;
	}

	private async Task<bool> Modificar(Producto producto)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		_context.Update(producto);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> Eliminar(Producto producto)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		return await _context.Productos
			.AsNoTracking()
			.Where(p => p.Id == producto.Id)
			.ExecuteDeleteAsync() > 0;
	}

	public async Task<bool> ExisteProducto(int id, string nombre)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		return await _context.Productos
			.AnyAsync(p => p.Id != id && p.Nombre!.ToLower() == nombre.ToLower());
	}

	public async Task<Producto?> Buscar(int id)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		return await _context.Productos
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
	}

	public async Task<List<Producto>> Listar(Expression<Func<Producto, bool>> criterio)
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		return await _context.Productos
			.AsNoTracking()
			.Where(criterio)
			.ToListAsync();
	}

	public async Task<List<Producto>> ListarProductos()
	{
		await using var _context = await DbFactory.CreateDbContextAsync();

		return await _context.Productos
			.AsNoTracking()
			.ToListAsync();
	}
}