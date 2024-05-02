using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventEatsQuotify.Interfaces;
using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace EventEatsQuotify.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AccountController> _logger;
        private readonly string _adminEmail = "maaz.munawer@outlook.com"; // Change this to the admin's email address

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IWebHostEnvironment hostEnvironment, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    // Check if the user is a vendor and if their registration is approved
                    bool isVendor = await _userManager.IsInRoleAsync(user, "Vendor");
                    bool isApproved = user.IsApproved;

                    if (isVendor && !isApproved)
                    {
                        ModelState.AddModelError("", "Your registration request is pending approval by the admin.");
                        return View(login);
                    }

                    // Proceed with password sign-in
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return await RedirectToDashboardAsync(user);
                    }
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(login);
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            var availableRoles = new List<string> { "Vendor", "Customer" };
            var model = new RegisterViewModel
            {
                AvailableRoles = availableRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            // Check if the selected role is Customer and only validate additional fields if so
            if (register.SelectedRole == "Customer")
            {
                ModelState.Remove("CNICImageFile");
                ModelState.Remove("BillingImageFile");
                ModelState.Remove("CNICImagePath");
                ModelState.Remove("BillingImagePath");
                ModelState.Remove("CNICNumber");
                ModelState.Remove("ShopAddress");
            }
            else if (register.SelectedRole == "Vendor")
            {
                ModelState.Remove("CNICImagePath");
                ModelState.Remove("BillingImagePath");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = register.Name,
                    Email = register.Email,
                    UserName = register.Email,
                    RegistrationDate = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync(register.SelectedRole))
                    {
                        if (register.SelectedRole == "Vendor")
                        {
                            try
                            {
                                // Save CNIC image
                                user.CNICImagePath = await SaveFile(register.CNICImageFile, "cnic_images");

                                // Save Billing image
                                user.BillingImagePath = await SaveFile(register.BillingImageFile, "billing_images");

                                user.CNICNumber = register.CNICNumber;
                                user.ShopAddress = register.ShopAddress;

                                // Update user with image paths
                                await _userManager.UpdateAsync(user);

                                // Add the selected role to the user
                                await _userManager.AddToRoleAsync(user, register.SelectedRole);

                                await SendRegistrationRequestEmailAsync(user);

                                // Set IsApproved to false for new vendor registrations
                                user.IsApproved = false;

                                // Set message for displaying pending request notification
                                TempData["RegistrationPendingMessage"] = "Your registration request as a vendor is pending approval.";

                                // Redirect to the home index
                                return RedirectToAction("Index", "Home");
                            }
                            catch (System.Net.Sockets.SocketException ex)
                            {
                                 await _userManager.DeleteAsync(user);

                                _logger.LogError(ex, "Error sending email: No such host is known.");
                                ModelState.AddModelError(string.Empty,"An error occurred while sending the email(Check your internet connection). Please try again later.");
                            }
                            catch (Exception ex)
                            {
                                await _userManager.DeleteAsync(user);

                                // Log the exception
                                _logger.LogError(ex, "Error occurred during vendor registration.");

                                // Add error message to ModelState
                                ModelState.AddModelError(string.Empty, "An error occurred during vendor registration. Please try again later.");
                            }
                        }
                        else if (register.SelectedRole == "Customer")
                        {
                            // Add the selected role to the user
                            await _userManager.AddToRoleAsync(user, register.SelectedRole);

                            // Sign in the user
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return await RedirectToDashboardAsync(user);
                        }
                        else
                        {
                            // Redirect to the home index
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Role {register.SelectedRole} does not exist.");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        // Check if the error is related to duplicate email
                        if (error.Code == "DuplicateEmail")
                        {
                            ModelState.AddModelError("", "Email address is already registered.");
                        }
                        else
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            register.AvailableRoles = new List<string> { "Vendor", "Customer" };
            return View(register);
        }

        private async Task<IActionResult> RedirectToDashboardAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Vendor"))
            {
                return RedirectToAction("VendorDashboard", "Vendor");
            }
            else if (roles.Contains("Customer"))
            {
                return RedirectToAction("CustomerDashboard", "Customer");
            }
            else if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        //private async Task SendRegistrationRequestEmailAsync(ApplicationUser user)
        //{
        //    var subject = "New Vendor Registration Request";
        //    var message = $"A new vendor registration request has been received from {user.Name}. Please review and approve.";
        //    await _emailSender.SendEmailAsync(_adminEmail, subject, message);
        //}
        private async Task SendRegistrationRequestEmailAsync(ApplicationUser user)
        {
            var subject = "New Vendor Registration Request";
            var message = $"A new vendor registration request has been received from {user.Name}. Please review and approve.";

            // Get the URL for the admin dashboard
            var adminDashboardUrl = Url.Action("AdminDashboard", "Admin", null, Request.Scheme);

            // Format the message body as HTML with a button
            var htmlMessage = $@"
        <html>
        <body>
            <p>{message}</p>
            <p>
                You can access the admin dashboard 
                    <a href=""{adminDashboardUrl}"" style=""text-decoration: none;"">
                    <button style=""background-color: #007bff; color: #ffffff; border: none; padding: 10px 20px; border-radius: 5px;"">Redirect to Dashboard</button>
                </a>
            </p>
        </body>
        </html>";

            try
            {
                // Send the email with the HTML-formatted message body
                await _emailSender.SendEmailAsync(_adminEmail, subject, htmlMessage);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                _logger.LogError(ex, "Error sending registration request email: No such host is known.");
                // Log the error and handle it gracefully
                // For example, you can set a flag to indicate that the email sending failed
                throw new Exception("An error occurred while sending the registration request email. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending registration request email.");
                // Log the error and handle it gracefully
                // For example, you can set a flag to indicate that the email sending failed
                throw new Exception("An unexpected error occurred. Please try again later.");
            }
        }




        private async Task<string> SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", folderName);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("/images", folderName, uniqueFileName); // Return relative path
        }

    }
}
