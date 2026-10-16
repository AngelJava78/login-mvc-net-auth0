namespace Auth0MvcDemo.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string? Auth0UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
