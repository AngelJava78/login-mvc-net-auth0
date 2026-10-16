using Auth0MvcDemo.DTOs;

namespace Auth0MvcDemo.Services;

public interface IBooksApiService
{
    Task<IReadOnlyList<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(CreateBookDto request);
    Task<bool> UpdateAsync(int id, UpdateBookDto request);
    Task<bool> DeleteAsync(int id);
}
