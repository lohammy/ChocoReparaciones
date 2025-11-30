using System.ComponentModel.DataAnnotations;

namespace ChocoReparaciones.Models
{
    public class Cliente
    {
		public int Id { get; set; }

		[Required(ErrorMessage = "El correo del cliente es obligatorio.")]
		[EmailAddress(ErrorMessage = "El correo ingresado no es válido.")]
		public string ClienteEmail { get; set; } = "";

		[Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
		[StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
		[RegularExpression(@"^[a-zA-Z\\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
		public string NombreCompleto { get; set; } = "";

		[StringLength(250, ErrorMessage = "La dirección no puede exceder los 250 caracteres.")]
		public string Direccion { get; set; } = "";

		[Required(ErrorMessage = "El número de teléfono es obligatorio.")]
		[Phone(ErrorMessage = "El número de teléfono no es válido.")]
		[Range(1, double.MaxValue, ErrorMessage = "Debe introducir un monto valido")]
		public string Telefono { get; set; } = "";

		[Required]
		public DateTime Fecha { get; set; } = DateTime.Now;

		[Range(1, double.MaxValue, ErrorMessage = "Debe introducir un monto valido")]
		public decimal Total { get; set; }

		[Required(ErrorMessage = "Debes seleccionar un método de pago.")]
		[RegularExpression("Efectivo|Tarjeta|Transferencia", ErrorMessage = "El método de pago no es válido.")]
		public string MetodoPago { get; set; } = "Efectivo";

		public string Productos { get; set; } = "";

		[Required]
		public string Estado { get; set; } = "Pendiente";
	}
}
