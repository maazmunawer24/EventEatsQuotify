using EventEatsQuotify.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.ViewModels
{
    public class QuotationRequestModel
    {
    
        public string VendorId { get; set; }

       
        public string QuotationOption { get; set; }

        public string AdditionalInstructions { get; set; }

       
        public string CustomerName { get; set; }

        public string VendorName { get; set; }

        public List<FoodItem> FoodItems { get; set; }

        public DateTime RequestDate { get; set; }
    }
}
