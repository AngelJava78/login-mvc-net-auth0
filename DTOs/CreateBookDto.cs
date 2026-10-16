using System.ComponentModel.DataAnnotations;

namespace Auth0MvcDemo.DTOs;

public class CreateBookDto
{
    [Required]
    [MaxLength(200)]
    [Display(Name = "Título")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Display(Name = "Autor")]
    public string Author { get; set; } = string.Empty;

    [Range(1, 9999)]
    [Display(Name = "Año de publicación")]
    public int PublicationYear { get; set; }
}
