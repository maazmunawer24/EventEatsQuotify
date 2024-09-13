using EventEatsQuotify.ContextDBConfig;
using EventEatsQuotify.Interfaces;
using EventEatsQuotify.Models;
using EventEatsQuotify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly EventEatsQuotifyDBContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public CustomerController(EventEatsQuotifyDBContext dbContext, UserManager<ApplicationUser> userManager,IEmailSender emailSender)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _emailSender = emailSender;
    }


    public async Task<IActionResult> CustomerDashboard(string? searchQuery = null, string searchType = "vendor")
    {
        try
        {
            var allUsers = await _dbContext.Users.ToListAsync();
            var approvedVendors = allUsers
                .Where(u => u.IsApproved &&
                            !_userManager.IsInRoleAsync(u, "Customer").Result &&
                            !_userManager.IsInRoleAsync(u, "Admin").Result);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower(); // Normalize the search query to lower case

                if (searchType == "vendor")
                {
                    approvedVendors = approvedVendors.Where(v => v.Name.ToLower().Contains(searchQuery)).ToList();
                }
                else if (searchType == "fooditem")
                {
                    var foodItems = await _dbContext.FoodItems
                        .Where(fi => fi.Name.ToLower().Contains(searchQuery))
                        .Select(fi => fi.VendorId)
                        .Distinct()
                        .ToListAsync();

                    approvedVendors = approvedVendors.Where(v => foodItems.Contains(v.Id)).ToList();
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_VendorList", approvedVendors.ToList());
            }

            return View(approvedVendors.ToList());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CustomerDashboard: {ex.Message}");
            return View("Error");
        }
    }



    [HttpGet]
    public JsonResult GetVendorProfile(string vendorId)
    {
        try
        {
            var vendor = _dbContext.Users.FirstOrDefault(u => u.Id == vendorId);
            if (vendor != null)
            {
                var vendorProfile = new
                {
                    vendor.Id,
                    vendor.Name,
                    vendor.Description,
                    vendor.ProfilePicture,
                    vendor.ShopAddress,
                    vendor.Website,
                    vendor.Specialties,
                    vendor.MenuHighlights,
                    vendor.CustomerReviews
                };
                return Json(new { success = true, data = vendorProfile });
            }
            else
            {
                return Json(new { success = false, message = "Vendor not found." });
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching vendor profile: {ex.Message}");
            return Json(new { success = false, message = "An error occurred while fetching the vendor profile." });
        }
    }


    [HttpGet]
    public JsonResult GetMenuItems(string vendorId)
    {
        try
        {
            var menuItems = _dbContext.FoodItems.Where(item => item.VendorId == vendorId).Select(item => new
            {
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.FoodPicturePath
            }).ToList();

            return Json(new { success = true, data = menuItems });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching menu items: {ex.Message}");
            return Json(new { success = false, message = "An error occurred while fetching the menu items." });
        }
    }


    [HttpGet]
    public JsonResult GetFoodItems(string vendorId)
    {
        try
        {
            var menuItems = _dbContext.FoodItems
                .Where(item => item.VendorId == vendorId)
                .Select(item => new
                {
                    item.Id,
                    item.Name
                })
                .ToList();

            return Json(new { success = true, data = menuItems });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching menu items: {ex.Message}");
            return Json(new { success = false, message = "An error occurred while fetching the menu items." });
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetQuotation(string vendorId)
    {
        var vendor = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == vendorId);
        if (vendor == null)
        {
            return NotFound("Vendor not found");
        }

        ViewBag.VendorId = vendor.Id;
        ViewBag.VendorName = vendor.Name; // Assuming "Name" is a property of your vendor model

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendQuotationRequest([FromBody] QuotationRequestModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.QuotationOption) || model.FoodItems == null || model.FoodItems.Count == 0)
            {
                ModelState.AddModelError("", "Please select a quotation type and add at least one food item.");
                return BadRequest(ModelState);
            }

            for (int i = 0; i < model.FoodItems.Count; i++)
            {
                ModelState.Remove($"FoodItems[{i}].Price");
                ModelState.Remove($"FoodItems[{i}].Category");

                
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // You can log these errors or return them in the response for debugging
                return BadRequest(new { errors });
            }


            var vendor = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == model.VendorId);
            if (vendor == null)
            {
                return NotFound("Vendor not found");
            }

            var quotationRequest = new QuotationRequest
            {
                CustomerId = _userManager.GetUserId(User),
                VendorId = model.VendorId,
                CustomerName = model.CustomerName,
                VendorName = model.VendorName,
                Status = "Pending",
                RequestDate = DateTime.UtcNow,
               AdditionalInstructions =model.AdditionalInstructions,
                VendorMessage = ""
            };

            foreach (var foodItem in model.FoodItems)
            {
                quotationRequest.QuotationFoodItems.Add(new QuotationFoodItem
                {
                    FoodItemId = foodItem.Id,
                    Name = foodItem.Name,
                    QuantityType = foodItem.QuantityType,
                    QuantityOrPersons = foodItem.QuantityOrPersons
                });
            }

            _dbContext.QuotationRequests.Add(quotationRequest);
            await _dbContext.SaveChangesAsync();

            var emailContent = @"
<html>
<head>
    <style>
        table { width: 100%; border-collapse: collapse; }
        th, td { border: 1px solid #dddddd; text-align: left; padding: 8px; }
        th { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <h2>Quotation Request Details</h2>
    <h3>Quotation Details : </h3>
    <table>
        <tr>
            <th>Food Items</th>
            <th>Quantity (KG)</th>
            <th>No of Persons</th>
            <th>Quotation Type</th>
        </tr>";

            foreach (var foodItem in model.FoodItems)
            {
                var quantity = foodItem.QuantityType.ToLower() == "quantity" ? $"{foodItem.QuantityOrPersons} KG" : "";
                var noOfPersons = foodItem.QuantityType.ToLower() == "persons" ? foodItem.QuantityOrPersons.ToString() : "";
                var quotationType = foodItem.QuantityType;
                emailContent += $@"
        <tr>
            <td>{foodItem.Name}</td>
            <td>{quantity}</td>
            <td>{noOfPersons}</td>
            <td>{quotationType}</td>
        </tr>";
            }

            emailContent += $@"
    </table>
    <h3>Additional Instructions : </h3>
    <p>{model.AdditionalInstructions}</p>
</body>
</html>";

            await SendQuotationEmailAsync("", "Quotation Request", emailContent);

            return Ok(new { success = true, message = "Quotation request sent successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending quotation request: {ex.Message}");
            return StatusCode(500, "An error occurred while sending the quotation request");
        }
    }

    private async Task SendQuotationEmailAsync(string emailAddress, string subject, string htmlMessage)
    {
        try
        {
            // Send the email with the HTML-formatted message body
            await _emailSender.SendEmailAsync(emailAddress, subject, htmlMessage);
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            // Log the error and handle it gracefully
            throw new Exception("An error occurred while sending the quotation request email. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            // Log the error and handle it gracefully
            throw new Exception("An unexpected error occurred while sending the quotation request email. Please try again later.", ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] Review review)
    {
        try
        {
            review.Id = Guid.NewGuid().ToString();
            review.CreatedAt = DateTime.UtcNow;
            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding review: {ex.Message}");
            return Json(new { success = false, message = "An error occurred while adding the review." });
        }
    }

    [HttpGet]
    public JsonResult GetReviews(string vendorId)
    {
        try
        {
            var reviews = _dbContext.Reviews.Where(r => r.VendorId == vendorId).OrderByDescending(r => r.CreatedAt).ToList();
            return Json(new { success = true, data = reviews });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching reviews: {ex.Message}");
            return Json(new { success = false, message = "An error occurred while fetching the reviews." });
        }
    }
    [HttpPost]
    public IActionResult CompareVendors(List<string> vendorIds)
    {
        try
        {
            // Fetch vendors
            var vendors = _dbContext.Users
                .Where(u => vendorIds.Contains(u.Id))
                .ToList();

            // Fetch food items
            var foodItems = _dbContext.FoodItems
                .Where(fi => vendorIds.Contains(fi.VendorId))
                .AsEnumerable() // Bring data into memory
                .GroupBy(fi => fi.Name)
                .Select(group => new FoodItemComparison
                {
                    FoodItemName = group.Key,
                    VendorPrices = group.ToDictionary(fi => fi.VendorId, fi => fi.Price)
                })
                .ToList();

            // Create view model
            var viewModel = new VendorComparisonViewModel
            {
                Vendors = vendors.Select(vendor => new VendorComparisonItem
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    Description = vendor.Description,
                    ProfilePicture = vendor.ProfilePicture,
                    ShopAddress = vendor.ShopAddress,
                    Website = vendor.Website,
                    Specialties = vendor.Specialties,
                    MenuHighlights = vendor.MenuHighlights,
                    CustomerReviews = vendor.CustomerReviews
                }).ToList(),
                FoodItems = foodItems
            };

            // Return partial view
            return PartialView("_VendorComparison", viewModel);
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return Json(new { success = false, message = "An error occurred while comparing the vendors." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewQuotations()
    {
        var customerId = _userManager.GetUserId(User);

        var quotations = await _dbContext.QuotationRequests
            .Where(q => q.CustomerId == customerId)
            .Include(q => q.QuotationFoodItems)
            .ThenInclude(qf => qf.FoodItem) // Include the related FoodItems through QuotationFoodItems
            .ToListAsync();

        // Pass the quotations to the view or partial view, including the RequestDate
        return PartialView("_ViewQuotationRequest", quotations);
    }





}
