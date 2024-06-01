namespace EventEatsQuotify.Models
{
    public class Review
    {
        public string Id { get; set; }
        public string VendorId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
