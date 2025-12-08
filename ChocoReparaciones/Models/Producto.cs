using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChocoReparaciones.Models
{
	public class Producto
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre del producto es obligatorio.")]
		[StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
		public string? Nombre { get; set; }

		[Required(ErrorMessage = "La descripción es obligatoria.")]
		[StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
		public string? Descripcion { get; set; }

		[Required(ErrorMessage = "La categoría es obligatoria.")]
		public string? Categoria { get; set; }

		[Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
		public decimal Precio { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa.")]
		public int CantidadDisponible { get; set; }

		public string? ImagenUrl { get; set; }

		public virtual ICollection<ReparacionDetalle> ReparacionDetalles { get; set; } = new List<ReparacionDetalle>();
	}
}