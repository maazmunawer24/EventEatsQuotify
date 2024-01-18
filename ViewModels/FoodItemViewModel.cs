using EventEatsQuotify.Models;
using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.ViewModels
{
    public class FoodItemViewModel
    {
        public List<FoodItem> FoodItems { get; set; } // Add this property
        public List<SelectedFoodItem> SelectedFoodItems { get; set; }
        public List<SelectedFoodItem> SelectedQuantities { get; set; }
        public List<ApplicationUser> Vendors { get; set; }

    }

    public class SelectedFoodItem
    {
        public int FoodItemId { get; set; }
        public decimal SelectedQuantity { get; set; }
    }
}
