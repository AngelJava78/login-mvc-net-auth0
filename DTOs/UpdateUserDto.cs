using System.ComponentModel.DataAnnotations;

namespace Auth0MvcDemo.DTOs;

public class UpdateUserDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Display(Name = "Auth0 User ID")]
    public string Auth0UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(320)]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool IsActive { get; set; }
}
