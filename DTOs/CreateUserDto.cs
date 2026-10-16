using System.ComponentModel.DataAnnotations;

namespace Auth0MvcDemo.DTOs;

public class CreateUserDto
{
    [Required]
    [MaxLength(150)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(320)]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool IsActive { get; set; } = true;
}
