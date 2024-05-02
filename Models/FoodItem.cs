using System.Collections.Generic;
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

        [Display(Name = "VendorId")]
        public string? VendorId { get; set; }  // Make it nullable

        [Display(Name = "Vendor")]
        public ApplicationUser? Vendor { get; set; }  // Make it nullable

        [Display(Name = "Category")]
        public string Category { get; set; } // Category of the menu item (e.g., starter, main course, dessert, beverage)

        // This property will store the path to the food picture in the file system
        [Display(Name = "Food Picture Path")]
        public string? FoodPicturePath { get; set; } // Make it nullable

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

        // property to store quantity or number of persons
        [Display(Name = "Quantity or No of Persons")]
        public int QuantityOrPersons { get; set; }

        // property to store the type of quantity (e.g., KG, Persons)
        [Display(Name = "Quantity Type")]
        public string QuantityType { get; set; }
    }

}

