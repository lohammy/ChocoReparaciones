using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ChocoReparaciones.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		[Required]
		[MaxLength(100)]
		public string NombreCompleto { get; set; } = "";

		[Required]
		[MaxLength(250)]
		public string Direccion { get; set; } = "";

		[Required]
		[Phone]
		public string Telefono { get; set; } = "";
	}

}
