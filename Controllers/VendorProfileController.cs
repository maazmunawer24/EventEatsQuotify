using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using EventEatsQuotify.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using EventEatsQuotify.ContextDBConfig;

namespace EventEatsQuotify.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class VendorProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly EventEatsQuotifyDBContext _context;  // Add this line

        public VendorProfileController(UserManager<ApplicationUser> userManager, EventEatsQuotifyDBContext context)  // Add EventEatsQuotifyDBContext to the constructor
        {
            _userManager = userManager;
            _context = context;  // Assign the injected context to the private variable
        }

        // GET: /VendorProfile
        public async Task<IActionResult> Index()
        {
            // Retrieve the current vendor's profile
            var vendor = await _userManager.GetUserAsync(User);

            var viewModel = new VendorProfileViewModel
            {
                // Map vendor properties to the view model
                BusinessName = vendor.Name,
                ContactEmail = vendor.Email,
                ContactPhone = vendor.PhoneNumber,
                Location = vendor.Address,
                Website = vendor.Website,
                Specialties = vendor.Specialties,
                Description = vendor.Description,
                MenuHighlights = vendor.MenuHighlights,
                CustomerReviews = vendor.CustomerReviews,
                ProfilePicture = vendor.ProfilePicture
                // Add more properties as needed
            };

            return View(viewModel);
        }

        // GET: /VendorProfile/Edit
        public async Task<IActionResult> Edit()
        {
            // Similar to the Index action, retrieve the vendor's profile
            var vendor = await _userManager.GetUserAsync(User);

            var viewModel = new VendorProfileViewModel
            {
                // Map vendor properties to the view model
                BusinessName = vendor.Name,
                ContactEmail = vendor.Email,
                ContactPhone = vendor.PhoneNumber,
                Location = vendor.Address,
                Website = vendor.Website,
                Specialties = vendor.Specialties,
                Description = vendor.Description,
                MenuHighlights = vendor.MenuHighlights,
                CustomerReviews = vendor.CustomerReviews,
                ProfilePicture = vendor.ProfilePicture
                // Add more properties as needed
            };

            return View(viewModel);
        }

        // POST: /VendorProfile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VendorProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current vendor's profile
                var vendor = await _userManager.GetUserAsync(User);

                // Update vendor properties based on the view model
                vendor.Name = viewModel.BusinessName;
                vendor.Email = viewModel.ContactEmail;
                vendor.PhoneNumber = viewModel.ContactPhone;
                vendor.Address = viewModel.Location;
                vendor.Website = viewModel.Website;
                vendor.Description = viewModel.Description;
                vendor.Specialties = viewModel.Specialties;
                vendor.CustomerReviews = viewModel.CustomerReviews;
                vendor.MenuHighlights = viewModel.MenuHighlights;
                //vendor.ProfilePicture = viewModel.ProfilePicture;

                // Handle Profile Picture upload
                if (viewModel.ProfilePictureFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.ProfilePictureFile.CopyToAsync(memoryStream);
                        vendor.ProfilePicture = memoryStream.ToArray();
                    }
                }

                // Update the vendor's profile in the database
                var result = await _userManager.UpdateAsync(vendor);

                if (result.Succeeded)
                {
                    // Redirect to the profile page after successful update
                    return RedirectToAction("Index");
                }

                // If the update fails, add errors to the ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    // Inspect 'errors' in the debugger to see specific validation issues
                    // ...
                }
            }

            // If the model state is not valid, return to the edit page with validation errors

            // Retrieve the vendor's profile again with updated food items
            var updatedVendor = await _userManager.GetUserAsync(User);
            viewModel.FoodItems = _context.FoodItems.Where(f => f.VendorId == updatedVendor.Id).ToList();

            return View(viewModel);
        }

    }
}


