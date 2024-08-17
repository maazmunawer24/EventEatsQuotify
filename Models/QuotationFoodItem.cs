using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.Models
{
    public class QuotationFoodItem
    {
        public int Id { get; set; }

        [Required]
        public int FoodItemId { get; set; }

        [Required]
        public int QuotationRequestId { get; set; }

        [Required(ErrorMessage = "The Food Item Name field is required.")]
        public string Name { get; set; }

        [Display(Name = "Quantity or No of Persons")]
        public int QuantityOrPersons { get; set; }

        [Display(Name = "Quantity Type")]
        public string? QuantityType { get; set; }

        // Navigation properties
        public FoodItem FoodItem { get; set; }
        public QuotationRequest QuotationRequest { get; set; }
    }
}
