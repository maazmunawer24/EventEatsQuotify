using EventEatsQuotify.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEatsQuotify.ViewModels
{
    public class VendorProfileViewModel
    {
        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }

        [Phone]
        [Display(Name = "Contact Phone")]
        public string? ContactPhone { get; set; }

        // Additional properties
        [Display(Name = "Location")]
        public string Location { get; set; }

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

        [NotMapped]
        [Display(Name = "Upload Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }

        public List<FoodItem>? FoodItems { get; set; }


    }
}
