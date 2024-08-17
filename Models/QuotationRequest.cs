using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEatsQuotify.Models
{
    public class QuotationRequest
    {
        public int Id { get; set; }

        [Required]
        public string VendorId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string VendorName { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<QuotationFoodItem> QuotationFoodItems { get; set; } = new List<QuotationFoodItem>();

        [Required]
        public DateTime RequestDate { get; set; }
    }
}
