using Microsoft.AspNetCore.Mvc;
using EventEatsQuotify.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using EventEatsQuotify.ContextDBConfig;

namespace EventEatsQuotify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventEatsQuotifyDBContext _dbContext;

        public HomeController(ILogger<HomeController> logger, EventEatsQuotifyDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: /Home/Vendors
        public async Task<IActionResult> Vendors()
        {
            // Retrieve vendors from the database
            var vendors = await _dbContext.Users.Where(u => u.IsApproved).ToListAsync();

            // Return vendors as JSON data
            return Json(vendors);
        }

    }
}