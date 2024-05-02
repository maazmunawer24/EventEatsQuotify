using EventEatsQuotify.Models;

namespace EventEatsQuotify.ViewModels
{
    public class QuotationRequestModel
    {
        public string VendorId { get; set; }
        public string QuotationOption { get; set; }
        public string AdditionalInstructions { get; set; }

        public List<FoodItem> FoodItems { get; set; } // Add this property


    }
}