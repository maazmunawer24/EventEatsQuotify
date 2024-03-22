using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventEatsQuotify.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            var pendingRequests = await GetPendingVendorRegistrationRequests();
            return View(pendingRequests);
        }

        private async Task<List<RegisterViewModel>> GetPendingVendorRegistrationRequests()
        {
            var users = await _userManager.Users.ToListAsync();

            // Filter users who are pending approval as vendors and are not in roles "Customer" or "Admin"
            var pendingVendorRequests = users.Where(u => !u.IsApproved &&
                                                          _userManager.IsInRoleAsync(u, "Vendor").Result &&
                                                         !_userManager.IsInRoleAsync(u, "Customer").Result &&
                                                         !_userManager.IsInRoleAsync(u, "Admin").Result);

            if (pendingVendorRequests != null)
            {
                var pendingRequestsViewModels = pendingVendorRequests.Select(u => new RegisterViewModel
                {
                    Name = u.Name,
                    Email = u.Email,
                    CNICNumber = u.CNICNumber,
                    ShopAddress = u.ShopAddress,
                    SelectedRole = "Vendor",
                    CNICImagePath = u.CNICImagePath,
                    BillingImagePath = u.BillingImagePath,
                }).ToList();

                return pendingRequestsViewModels;
            }
            else
            {
                return new List<RegisterViewModel>();
            }
        }


        public async Task<IActionResult> Approve(string userId)
        {
            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Set the IsApproved property to true
            user.IsApproved = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
               
                return RedirectToAction("AdminDashboard");
                
            }

            ViewBag.ErrorMessage = "Failed to approve vendor registration.";
            return View("Error");
        }

        public async Task<IActionResult> Reject(string userId)
        {
            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("AdminDashboard");
            }

            ViewBag.ErrorMessage = "Failed to reject vendor registration.";
            return View("Error");
        }
    }
}
