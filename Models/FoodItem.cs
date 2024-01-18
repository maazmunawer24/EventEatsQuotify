using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.Models
{
    public class FoodItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Food Item Name field is required.")]
        [Display(Name = "Food Item Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Price Per KG field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price Per KG")]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }  // Make it nullable

        [Display(Name = "Vendor")]
        public string? VendorId { get; set; }  // Make it nullable

        [Display(Name = "Vendor")]
        public ApplicationUser? Vendor { get; set; }  // Make it nullable

        [Display(Name = "Food Picture")]
        public byte[]? FoodPicture { get; set; }  // Make it nullable

        // Add more properties as needed
    }

}
