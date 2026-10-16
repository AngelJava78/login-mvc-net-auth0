using System.Net;
using System.Net.Http.Json;
using Auth0MvcDemo.DTOs;

namespace Auth0MvcDemo.Services;

public class BooksApiService : IBooksApiService
{
    private readonly HttpClient _httpClient;

    public BooksApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<BookDto>> GetAllAsync()
    {
        var books = await _httpClient.GetFromJsonAsync<List<BookDto>>("api/books");
        return books ?? [];
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/books/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookDto>();
    }

    public async Task<BookDto> CreateAsync(CreateBookDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/books", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BookDto>()
            ?? throw new InvalidOperationException("La API no devolvió el libro creado.");
    }

    public async Task<bool> UpdateAsync(int id, UpdateBookDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/books/{id}", request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/books/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }
}
