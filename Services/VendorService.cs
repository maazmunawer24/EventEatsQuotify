using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Identity;

namespace EventEatsQuotify.Services
{
    public class VendorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public VendorService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> GetVendorAsync(string vendorId)
        {
            return await _userManager.FindByIdAsync(vendorId);
        }

        public async Task<List<ApplicationUser>> GetAllVendors()
        {
            // Retrieve and return the list of all users with the "Vendor" role
            var vendorRole = await _roleManager.FindByNameAsync("Vendor");

            if (vendorRole != null)
            {
                return _userManager.GetUsersInRoleAsync(vendorRole.Name).Result.ToList();
            }

            return new List<ApplicationUser>();
        }

        // Add more vendor-related methods as needed
    }
}
