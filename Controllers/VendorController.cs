using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventEatsQuotify.Controllers
{
    public class VendorController : Controller
    {
        [Authorize(Roles = "Vendor")]
        public IActionResult VendorDashboard()
        {
            // Actions related to the vendor role
            return View();
        }
    }
}