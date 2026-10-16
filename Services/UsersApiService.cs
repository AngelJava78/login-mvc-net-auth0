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

    public async Task<string?> CreateAsync(CreateUserDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users", request);

        if (response.IsSuccessStatusCode)
            return null;

        // 400 y 409 son errores de negocio/validacion que deben mostrarse
        // en la vista, no convertirse en una excepcion tecnica de HttpClient.
        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Conflict)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorDto>();

            return !string.IsNullOrWhiteSpace(error?.Message)
                ? error.Message
                : $"No fue posible crear el usuario. HTTP {(int)response.StatusCode}.";
        }

        response.EnsureSuccessStatusCode();
        return null;
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
