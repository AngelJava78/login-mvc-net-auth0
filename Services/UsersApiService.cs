using System.Net;
using System.Net.Http.Json;
using Auth0MvcDemo.DTOs;

namespace Auth0MvcDemo.Services;

public class UsersApiService : IUsersApiService
{
    private readonly HttpClient _httpClient;

    public UsersApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync()
    {
        var users = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users");
        return users ?? [];
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/users/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<UserDto> CreateAsync(CreateUserDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDto>()
            ?? throw new InvalidOperationException("La API no devolvió el usuario creado.");
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/users/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }
}
