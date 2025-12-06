using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChocoReparaciones.Models
{
    public class Producto
    {
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre del producto es obligatorio.")]
		[StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
		[RegularExpression(@"^[a-zA-Z\\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
		public string? Nombre { get; set; }

		[Required(ErrorMessage = "La descripción es obligatoria.")]
		[StringLength(30, ErrorMessage = "La descripción no puede superar los 30 caracteres.")]
		public string? Descripcion { get; set; }

		[Required(ErrorMessage = "La categoría es obligatoria.")]
		public string? Categoria { get; set; }

		[Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
		public decimal Precio { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa.")]
		public int CantidadDisponible { get; set; }

		public string? ImagenUrl { get; set; }

		[NotMapped]
		[Range(1, int.MaxValue, ErrorMessage = "Debe ingresar una cantidad válida para agregar stock.")]
		public int StockExtra { get; set; } = 1;
	}
}
