using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Authorization;
using EventEatsQuotify.Interfaces;

namespace EventEatsQuotify.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager,  IEmailSender emailSender, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
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
                    CNICBackImagePath = u.CNICBackImagePath,
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
                try
                {
                    await SendApprovalEmailAsync(user);
                    return RedirectToAction("AdminDashboard");
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    // Revert the approval status if email sending fails due to network issues
                    user.IsApproved = false;
                    await _userManager.UpdateAsync(user);

                    _logger.LogError(ex, "Error sending email: No such host is known.");
                    ModelState.AddModelError(string.Empty, "An error occurred while sending the email (Check your internet connection). Please try again later.");
                }
                catch (Exception ex)
                {
                    // Log the specific error message from the email sending service
                    _logger.LogError(ex, "Error sending approval email.");

                    // Revert the approval status if email sending fails for any other reason
                    user.IsApproved = false;
                    await _userManager.UpdateAsync(user);

                    // Add error message to ModelState
                    ModelState.AddModelError(string.Empty, "An error occurred while sending the email. Please try again later.");
                }
            }
            else
            {
                // Revert the approval status if user update fails
                user.IsApproved = false;
                await _userManager.UpdateAsync(user);

                ViewBag.ErrorMessage = "Failed to approve vendor registration.";
                return View("Error");
            }

            // Revert the approval status if the control reaches here
            user.IsApproved = false;
            await _userManager.UpdateAsync(user);

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

        private async Task SendApprovalEmailAsync(ApplicationUser user)
        {
            var subject = "Vendor Registration Request Approval";
            var message = $"Your vendor registration request has been approved.";

            // Get the URL for the admin dashboard
            var VendorDashboardUrl = Url.Action("VendorDashboard", "Vendor", null, Request.Scheme);

            // Format the message body as HTML with a button
            var htmlMessage = $@"
        <html>
        <body>
            <p>{message}</p>
            <p>
                You can access the vendor dashboard 
                    <a href=""{VendorDashboardUrl}"" style=""text-decoration: none;"">
                    <button style=""background-color: #007bff; color: #ffffff; border: none; padding: 10px 20px; border-radius: 5px;"">Redirect to Dashboard</button>
                </a>
            </p>
        </body>
        </html>";

            try
            {
                // Send the email with the HTML-formatted message body
                await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                _logger.LogError(ex, "Error sending registration response email: No such host is known.");
                // Log the error and handle it gracefully
                throw new Exception("An error occurred while sending the registration response email. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending registration response email.");
                // Log the error and handle it gracefully
                throw new Exception("An unexpected error occurred. Please try again later.");
            }
        }


    }
}
