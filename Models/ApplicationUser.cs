using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        // Additional properties

        [Display(Name = "Website")]
        [Url]
        public string? Website { get; set; }

        [Display(Name = "Specialties")]
        public string? Specialties { get; set; }

        // Additional sections
        [Display(Name = "About Us")]
        public string? Description { get; set; }

        [Display(Name = "Menu Highlights")]
        public string? MenuHighlights { get; set; }

        [Display(Name = "Customer Reviews")]
        public string? CustomerReviews { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;


    }
}
