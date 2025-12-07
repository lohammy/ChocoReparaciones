using System.ComponentModel.DataAnnotations;

namespace ChocoReparaciones.Models
{
	public class Reparacion
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
		[RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Solo se permiten letras.")]
		public string ClienteNombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El número de teléfono es obligatorio.")]
		[Phone(ErrorMessage = "El número de teléfono no es válido.")]
		[StringLength(15, MinimumLength = 9, ErrorMessage = "El teléfono debe tener entre 9 y 15 caracteres.")]
		public string Telefono { get; set; } = "";

		[Required(ErrorMessage = "La descripción del equipo es obligatoria.")]
		public string DescripcionEquipo { get; set; } = string.Empty;

		[Required(ErrorMessage = "El problema reportado es obligatorio.")]
		public string ProblemaReportado { get; set; } = string.Empty;

		public DateTime FechaIngreso { get; set; } = DateTime.Now;

		public string Estado { get; set; } = "Pendiente";

		public string? TecnicoAsignadoId { get; set; }

		public string? ComentarioTecnico { get; set; }

		[Required(ErrorMessage = "Debe seleccionar una prioridad.")]
		public string Prioridad { get; set; } = "Media";

		[Required]
		public DateTime FechaEntrega { get; set; } = DateTime.Today.AddDays(3);

		public virtual ICollection<ReparacionDetalle> ReparacionDetalles { get; set; } = new List<ReparacionDetalle>();
	}
}