using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Name")]
        public string Name { get; set; } = "DefaultName";

        [Display(Name = "ShopAddress")]
        public string ShopAddress { get; set; } = "DefaultAddress";

        [Display(Name = "Website")]
        [Url]
        public string? Website { get; set; }

        [Display(Name = "Specialties")]
        public string? Specialties { get; set; }

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

        // New fields for admin approval and CNIC verification
        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        // Property for storing CNIC image path
        public string? CNICImagePath { get; set; }

        public string? CNICBackImagePath { get; set; }

        // Property for storing billing image path
        public string? BillingImagePath { get; set; }

        // Property for CNIC Number
        [Display(Name = "CNIC Number")]
        public string? CNICNumber { get; set; }

    }
}
