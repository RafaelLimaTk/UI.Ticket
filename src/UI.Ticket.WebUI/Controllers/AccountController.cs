using Microsoft.AspNetCore.Mvc;

namespace UI.Ticket.WebUI.Controllers;
public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}
