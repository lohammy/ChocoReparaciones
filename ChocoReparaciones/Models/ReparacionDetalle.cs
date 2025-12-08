using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChocoReparaciones.Models
{
	public class ReparacionDetalle
	{
		[Key]
		public int Id { get; set; }

		public int ReparacionId { get; set; }

		public int ProductoId { get; set; }

		[Required(ErrorMessage = "Este campo es obligatorio")]
		[Range(1, double.MaxValue, ErrorMessage = "Debe introducir una cantidad válida")]
		public int Cantidad { get; set; }

		[Required(ErrorMessage = "Este campo es obligatorio")]
		[Range(1, double.MaxValue, ErrorMessage = "Debe introducir un monto válido")]
		public decimal Precio { get; set; }

		// Navigation Properties
		[ForeignKey("ReparacionId")]
		[InverseProperty("ReparacionDetalles")]
		public virtual Reparacion Reparacion { get; set; } = null!;

		[ForeignKey("ProductoId")]
		[InverseProperty("ReparacionDetalles")]
		public virtual Producto Producto { get; set; } = null!;
	}
}