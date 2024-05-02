using System.ComponentModel.DataAnnotations.Schema;

namespace EventEatsQuotify.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string FilePath { get; set; }

        // Foreign key
        public int FoodItemId { get; set; }

        // Navigation property
        [ForeignKey("FoodItemId")]
        public FoodItem FoodItem { get; set; }  // Navigation property to FoodItem

        // Add any other properties as needed
    }
}
