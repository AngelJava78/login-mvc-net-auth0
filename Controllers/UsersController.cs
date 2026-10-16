using Auth0MvcDemo.DTOs;
using Auth0MvcDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth0MvcDemo.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly IUsersApiService _usersApiService;

    public UsersController(IUsersApiService usersApiService)
    {
        _usersApiService = usersApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _usersApiService.GetAllAsync();
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        await _usersApiService.CreateAsync(request);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _usersApiService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        var model = new UpdateUserDto
        {
            Id = user.Id,
            Auth0UserId = user.Auth0UserId,
            Name = user.Name,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateUserDto request)
    {
        if (id != request.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(request);

        var updated = await _usersApiService.UpdateAsync(id, request);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _usersApiService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _usersApiService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
