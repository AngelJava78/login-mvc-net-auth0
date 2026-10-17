using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth0MvcDemo.DTOs;
using Auth0MvcDemo.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace Auth0MvcDemo.Services;

public class BooksApiService : IBooksApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BooksApiService(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IReadOnlyList<BookDto>> GetAllAsync()
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.GetAsync("api/books");
        ThrowIfForbidden(response);
        response.EnsureSuccessStatusCode();

        var books = await response.Content.ReadFromJsonAsync<List<BookDto>>();
        return books ?? [];
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.GetAsync($"api/books/{id}");
        ThrowIfForbidden(response);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookDto>();
    }

    public async Task<BookDto> CreateAsync(CreateBookDto request)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.PostAsJsonAsync("api/books", request);
        ThrowIfForbidden(response);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BookDto>()
            ?? throw new InvalidOperationException("La API no devolvió el libro creado.");
    }

    public async Task<bool> UpdateAsync(int id, UpdateBookDto request)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.PutAsJsonAsync($"api/books/{id}", request);
        ThrowIfForbidden(response);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await SetAccessTokenAsync();

        var response = await _httpClient.DeleteAsync($"api/books/{id}");
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
