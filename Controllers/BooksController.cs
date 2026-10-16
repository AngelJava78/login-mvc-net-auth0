using Auth0MvcDemo.DTOs;
using Auth0MvcDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth0MvcDemo.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly IBooksApiService _booksApiService;

    public BooksController(IBooksApiService booksApiService)
    {
        _booksApiService = booksApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var books = await _booksApiService.GetAllAsync();
        return View(books);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateBookDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        await _booksApiService.CreateAsync(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _booksApiService.GetByIdAsync(id);

        if (book is null)
            return NotFound();

        var model = new UpdateBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            PublicationYear = book.PublicationYear
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateBookDto request)
    {
        if (id != request.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(request);

        var updated = await _booksApiService.UpdateAsync(id, request);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _booksApiService.GetByIdAsync(id);

        if (book is null)
            return NotFound();

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _booksApiService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
