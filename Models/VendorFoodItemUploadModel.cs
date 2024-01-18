using EventEatsQuotify.Models;
using System.Collections.Generic;

namespace EventEatsQuotify.Models
{
    public class VendorFoodItemUploadModel
    {
        public List<FoodItem> FoodItems { get; set; }
        //public List<ApplicationUser> Vendors { get; set; } // Add this property
    }
}
