using System.ComponentModel.DataAnnotations;

namespace ChocoReparaciones.Models;

public class Usuario { 
    public int Id { get; set; }

	[Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres.")]
    [RegularExpression(@"^[a-zA-Z\\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]

    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un rol para el usuario.")]
    public string Rol { get; set; } = string.Empty;
    

    
}
