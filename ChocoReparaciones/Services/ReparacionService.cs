using ChocoReparaciones.Data;
using ChocoReparaciones.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChocoReparaciones.Services;

public class ReparacionServices(IDbContextFactory<ApplicationDbContext> DbFactory)
{
	public async Task<bool> Guardar(Reparacion reparacion)
	{
		if (!await Existe(reparacion.Id))
		{
			return await Insertar(reparacion);
		}
		else
		{
			return await Modificar(reparacion);
		}
	}

	private async Task<bool> Existe(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Reparaciones.AnyAsync(r => r.Id == id);
	}

	private async Task<bool> Insertar(Reparacion reparacion)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		foreach (var detalle in reparacion.ReparacionDetalles)
		{
			var producto = await contexto.Productos
				.FirstOrDefaultAsync(p => p.Id == detalle.ProductoId);

			if (producto == null)
			{
				return false;
			}

			if (producto.CantidadDisponible < detalle.Cantidad)
			{
				return false;
			}

			producto.CantidadDisponible -= detalle.Cantidad;
			contexto.Productos.Update(producto);
			await contexto.SaveChangesAsync();
		}

		contexto.Reparaciones.Add(reparacion);
		return await contexto.SaveChangesAsync() > 0;
	}

	private async Task<bool> Modificar(Reparacion reparacion)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		var detallesAnteriores = await contexto.ReparacionDetalles
			.Where(d => d.ReparacionId == reparacion.Id)
			.ToListAsync();

		foreach (var detalleAnterior in detallesAnteriores)
		{
			var producto = await contexto.Productos
				.FirstOrDefaultAsync(p => p.Id == detalleAnterior.ProductoId);

			if (producto != null)
			{
				producto.CantidadDisponible += detalleAnterior.Cantidad;
				contexto.Productos.Update(producto);
				await contexto.SaveChangesAsync();
			}
		}

		contexto.ReparacionDetalles.RemoveRange(detallesAnteriores);
		await contexto.SaveChangesAsync();

		foreach (var detalle in reparacion.ReparacionDetalles)
		{
			var producto = await contexto.Productos
				.FirstOrDefaultAsync(p => p.Id == detalle.ProductoId);

			if (producto == null)
			{
				return false;
			}

			if (producto.CantidadDisponible < detalle.Cantidad)
			{
				return false;
			}

			producto.CantidadDisponible -= detalle.Cantidad;
			contexto.Productos.Update(producto);
			await contexto.SaveChangesAsync();

			detalle.Id = 0;
		}

		contexto.Reparaciones.Update(reparacion);
		return await contexto.SaveChangesAsync() > 0;
	}

	public async Task<bool> Eliminar(int reparacionId)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		var detalles = await BuscarDetalles(reparacionId);

		foreach (var detalle in detalles)
		{
			var producto = await contexto.Productos
				.FirstOrDefaultAsync(p => p.Id == detalle.ProductoId);

			if (producto != null)
			{
				producto.CantidadDisponible += detalle.Cantidad;
				contexto.Productos.Update(producto);
				await contexto.SaveChangesAsync();
			}
		}

		var reparacion = await contexto.Reparaciones
			.Include(r => r.ReparacionDetalles)
			.FirstOrDefaultAsync(r => r.Id == reparacionId);

		if (reparacion == null) return false;

		contexto.ReparacionDetalles.RemoveRange(reparacion.ReparacionDetalles);
		contexto.Reparaciones.Remove(reparacion);

		return await contexto.SaveChangesAsync() > 0;
	}

	public async Task<Reparacion?> Buscar(int reparacionId)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Reparaciones
			.Include(r => r.ReparacionDetalles)
			.FirstOrDefaultAsync(r => r.Id == reparacionId);
	}

	public async Task<List<ReparacionDetalle>> BuscarDetalles(int reparacionId)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.ReparacionDetalles
			.Include(d => d.Producto)
			.AsNoTracking()
			.Where(d => d.ReparacionId == reparacionId)
			.ToListAsync();
	}

	public async Task<List<Reparacion>> Listar(Expression<Func<Reparacion, bool>> criterio)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Reparaciones
			.Include(r => r.ReparacionDetalles)
			.AsNoTracking()
			.Where(criterio)
			.ToListAsync();
	}

	public async Task<Producto?> BuscarProducto(int id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Productos
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Id == id);
	}

	public async Task<List<Producto>> ListarProductos()
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Productos
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<bool> EliminarDetalle(int detalleId)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();

		var detalle = await contexto.ReparacionDetalles
			.FirstOrDefaultAsync(d => d.Id == detalleId);

		if (detalle == null) return false;

		var producto = await contexto.Productos
			.FirstOrDefaultAsync(p => p.Id == detalle.ProductoId);

		if (producto != null)
		{
			producto.CantidadDisponible += detalle.Cantidad;
			contexto.Productos.Update(producto);
		}

		contexto.ReparacionDetalles.Remove(detalle);
		return await contexto.SaveChangesAsync() > 0;
	}
}