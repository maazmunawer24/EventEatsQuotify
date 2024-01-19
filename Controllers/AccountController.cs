using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EventEatsQuotify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EventEatsQuotify.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

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
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        // Redirect based on user role
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
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            // Define roles
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
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = register.Name,
                    Address = register.Address,
                    Email = register.Email,
                    UserName = register.Name,
                    RegistrationDate = DateTime.UtcNow
                };
                Console.WriteLine($"SelectedRole: {register.SelectedRole}");
                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    // Check if the selected role exists before adding the user to it
                    var roleExists = await _roleManager.RoleExistsAsync(register.SelectedRole);

                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, register.SelectedRole);

                        await _signInManager.SignInAsync(user, false);
                        return await RedirectToDashboardAsync(user);
                    }
                    else
                    {
                        // Log an error or handle the situation where the role doesn't exist
                        ModelState.AddModelError("", $"Role {register.SelectedRole} does not exist.");
                    }
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }

            // Validation failed or role doesn't exist
            // Repopulate the roles dropdown
            register.AvailableRoles = new List<string> { "Vendor", "Customer" };

            return View(register);
        }

        private async Task<IActionResult> RedirectToDashboardAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Vendor"))
            {
                // Placeholder for Vendor Dashboard
                return RedirectToAction("VendorDashboard", "Vendor");
            }
            else if (roles.Contains("Customer"))
            {
                // Placeholder for Customer Dashboard
                return RedirectToAction("CustomerDashboard", "Customer");
            }

            // Default redirection for other roles or scenarios
            return RedirectToAction("Index", "Home");
        }
    }
}
