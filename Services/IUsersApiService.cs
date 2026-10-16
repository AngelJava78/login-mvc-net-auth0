using Auth0MvcDemo.DTOs;

namespace Auth0MvcDemo.Services;

public interface IUsersApiService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto request);
    Task<bool> UpdateAsync(int id, UpdateUserDto request);
    Task<bool> DeleteAsync(int id);
}
