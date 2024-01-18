using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventEatsQuotify.Controllers
{

public class CustomerController : Controller
{
    [Authorize(Roles = "Customer")]
    public IActionResult CustomerDashboard()
    {
        // Actions related to the customer role
        return View();
    }
}
}