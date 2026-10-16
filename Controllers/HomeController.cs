using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth0MvcDemo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    [Authorize]
    public IActionResult Profile() => View();
}
