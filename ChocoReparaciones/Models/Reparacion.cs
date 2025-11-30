using System.ComponentModel.DataAnnotations;

namespace ChocoReparaciones.Models
{
    public class Reparacion
    {
		public int Id { get; set; }

		[Required]
		[RegularExpression(@"^[a-zA-Z\\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
		public string ClienteNombre { get; set; } = string.Empty;

		[Required]
		[RegularExpression(@"^[a-zA-Z\\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
		public string DescripcionEquipo { get; set; } = string.Empty;

		[Required]
		[RegularExpression(@"^[a-zA-Z\\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
		public string ProblemaReportado { get; set; } = string.Empty;

		public DateTime FechaIngreso { get; set; } = DateTime.Now;

		public string Estado { get; set; } = "Pendiente";

		public string? TecnicoAsignadoId { get; set; }
		public string? ComentarioTecnico { get; set; }

		[Required(ErrorMessage = "Debe seleccionar una prioridad.")]
		public string Prioridad { get; set; } = "Media";

		[Required]
		public DateTime FechaEntrega { get; set; } = DateTime.Today.AddDays(3);
	}
}
