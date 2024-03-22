using EventEatsQuotify.ContextDBConfig;
using EventEatsQuotify.Models;
using EventEatsQuotify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventEatsQuotify.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EventEatsQuotifyDBContext _context;

        public CustomerProfileController(UserManager<ApplicationUser> userManager, EventEatsQuotifyDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /CustomerProfile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            //if (!user.IsApproved)
            //{
            //    // If the user is not approved by the admin, redirect to a page indicating pending approval
            //    return RedirectToAction("PendingApproval", "Home");
            //}

            var viewModel = new CustomerProfiileViewModel
            {
                Name = user.Name,
                ContactEmail = user.Email,
                ContactPhone = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                RegistrationDate = user.RegistrationDate

            };

            return View(viewModel);
        }

        // GET: /CustomerProfile/Edit
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            //if (!user.IsApproved)
            //{
            //    // If the user is not approved by the admin, redirect to a page indicating pending approval
            //    return RedirectToAction("PendingApproval", "Home");
            //}

            var viewModel = new CustomerProfiileViewModel
            {
                Name = user.Name,
                ContactEmail = user.Email,
                ContactPhone = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture             

            };

            return View(viewModel);
        }

        // POST: /CustomerProfile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerProfiileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                user.Name = viewModel.Name;
                user.Email = viewModel.ContactEmail;
                user.PhoneNumber = viewModel.ContactPhone;          

                if (viewModel.ProfilePictureFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.ProfilePictureFile.CopyToAsync(memoryStream);
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                }

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}
