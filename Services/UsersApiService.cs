using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth0MvcDemo.DTOs;
using Auth0MvcDemo.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace Auth0MvcDemo.Services;

public class UsersApiService : IUsersApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsersApiService(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync()
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.GetAsync("api/users");
        ThrowIfForbidden(response);
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        return users ?? [];
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.GetAsync($"api/users/{id}");
        ThrowIfForbidden(response);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<string?> CreateAsync(CreateUserDto request)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.PostAsJsonAsync("api/users", request);
        ThrowIfForbidden(response);

        if (response.IsSuccessStatusCode)
            return null;

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
        await SetAccessTokenAsync();

        var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", request);
        ThrowIfForbidden(response);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.DeleteAsync($"api/users/{id}");
        ThrowIfForbidden(response);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }

    private static void ThrowIfForbidden(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new ApiForbiddenException();
        }
    }

    private async Task SetAccessTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No hay un HttpContext disponible.");

        var accessToken = await httpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "No se encontró el Access Token del usuario autenticado. Cierra sesión e inicia sesión nuevamente.");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
    }
}
