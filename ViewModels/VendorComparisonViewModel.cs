public class VendorComparisonViewModel
{
    public List<VendorComparisonItem> Vendors { get; set; }
    public List<FoodItemComparison> FoodItems { get; set; }
}

public class VendorComparisonItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string ShopAddress { get; set; }
    public string Website { get; set; }
    public string Specialties { get; set; }
    public string MenuHighlights { get; set; }
    public string CustomerReviews { get; set; }
}

public class FoodItemComparison
{
    public string FoodItemName { get; set; }
    public Dictionary<string, decimal> VendorPrices { get; set; }
}
