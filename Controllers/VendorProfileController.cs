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
    [Authorize(Roles = "Vendor")]
    public class VendorProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EventEatsQuotifyDBContext _context;

        public VendorProfileController(UserManager<ApplicationUser> userManager, EventEatsQuotifyDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /VendorProfile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!user.IsApproved)
            {
                // If the user is not approved by the admin, redirect to a page indicating pending approval
                return RedirectToAction("PendingApproval", "Home");
            }

            var viewModel = new VendorProfileViewModel
            {
                BusinessName = user.Name,
                ContactEmail = user.Email,
                ContactPhone = user.PhoneNumber,
                Location = user.ShopAddress,
                Website = user.Website,
                Specialties = user.Specialties,
                Description = user.Description,
                MenuHighlights = user.MenuHighlights,
                CustomerReviews = user.CustomerReviews,
                ProfilePicture = user.ProfilePicture,
                RegistrationDate = user.RegistrationDate
            };

            return View(viewModel);
        }

        // GET: /VendorProfile/Edit
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!user.IsApproved)
            {
                // If the user is not approved by the admin, redirect to a page indicating pending approval
                return RedirectToAction("PendingApproval", "Home");
            }

            var viewModel = new VendorProfileViewModel
            {
                BusinessName = user.Name,
                ContactEmail = user.Email,
                ContactPhone = user.PhoneNumber,
                Location = user.ShopAddress,
                Website = user.Website,
                Specialties = user.Specialties,
                Description = user.Description,
                MenuHighlights = user.MenuHighlights,
                CustomerReviews = user.CustomerReviews,
                ProfilePicture = user.ProfilePicture
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VendorProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Check if the new email is different from the current email
                if (viewModel.ContactEmail != user.Email)
                {
                    // Check if the new email already exists in the database
                    var existingUser = await _userManager.FindByEmailAsync(viewModel.ContactEmail);
                    if (existingUser != null)
                    {
                        // Email already exists for another user
                        ModelState.AddModelError("ContactEmail", "The email is already in use.");
                        return View(viewModel);
                    }
                }

                user.Name = viewModel.BusinessName;
                user.PhoneNumber = viewModel.ContactPhone;
                user.ShopAddress = viewModel.Location;
                user.Website = viewModel.Website;
                user.Description = viewModel.Description;
                user.Specialties = viewModel.Specialties;
                user.CustomerReviews = viewModel.CustomerReviews;
                user.MenuHighlights = viewModel.MenuHighlights;
                user.Email = viewModel.ContactEmail;

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
