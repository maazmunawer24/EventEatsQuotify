using EventEatsQuotify.ContextDBConfig;
using EventEatsQuotify.Interfaces;
using EventEatsQuotify.Models;
using EventEatsQuotify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventEatsQuotify.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class VendorController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly EventEatsQuotifyDBContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public VendorController(IWebHostEnvironment environment, EventEatsQuotifyDBContext dbContext, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _environment = environment;
            _dbContext = dbContext;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult VendorDashboard()
        {
            // Actions related to the vendor role
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MenuItems(string sortOrder, string searchString)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not authenticated
            }

            var vendorId = user.Id;

            // Filter menu items based on the vendor ID
            var menuItems = _dbContext.FoodItems.Where(item => item.VendorId == vendorId);

            if (!String.IsNullOrEmpty(searchString))
            {
                menuItems = menuItems.Where(item => item.Name.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    menuItems = menuItems.OrderByDescending(item => item.Name);
                    break;
                case "Price":
                    menuItems = menuItems.OrderBy(item => item.Price);
                    break;
                case "price_desc":
                    menuItems = menuItems.OrderByDescending(item => item.Price);
                    break;
                default:
                    menuItems = menuItems.OrderBy(item => item.Name);
                    break;
            }

            return View(await menuItems.ToListAsync());
        }


        [HttpPost]
        public ActionResult DeleteSelectedMenuItems(List<int> selectedIds)
        {
            foreach (var id in selectedIds)
            {
                // Delete associated photos first
                var photos = _dbContext.Photos.Where(p => p.FoodItemId == id).ToList();
                _dbContext.Photos.RemoveRange(photos);

                // Then delete the FoodItem
                var foodItem = _dbContext.FoodItems.Find(id);
                if (foodItem != null)
                {
                    _dbContext.FoodItems.Remove(foodItem);
                }
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Handle exception
                // Log the exception details or return an error message
                return Json(new { success = false, message = "An error occurred while deleting the items." });
            }

            return Json(new { success = true });
        }


        [HttpGet]
        public IActionResult UploadMenuItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMenuItem(FoodItem model, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the vendor's ID (you need to implement this logic)
                    model.VendorId = await GetVendorId(); // Implement this method to retrieve the vendor's ID

                    // Check if the food item already exists (case-insensitive using ToLower())
                    bool foodItemExists = _dbContext.FoodItems
                        .Any(f => f.Name.ToLower() == model.Name.ToLower() && f.VendorId == model.VendorId);

                    if (foodItemExists)
                    {
                        ModelState.AddModelError("", "A food item with this name already exists.");
                        return View(model);
                    }

                    if (photo != null && photo.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "images/Foods/");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        model.FoodPicturePath = "/images/Foods/" + uniqueFileName;
                    }

                    _dbContext.FoodItems.Add(model);
                    await _dbContext.SaveChangesAsync();

                    // Redirect to a success page or return a success message
                    return RedirectToAction("MenuItems");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the menu item: " + ex.Message);
                }
            }

            // If the model state is not valid, return the upload form with validation errors
            return View(model);
        }


        [HttpPost]
        public IActionResult EditMenuItem(FoodItem model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing menu item from the database
                    var existingMenuItem = _dbContext.FoodItems.FirstOrDefault(item => item.Id == model.Id);

                    if (existingMenuItem != null)
                    {
                        // Update the properties of the existing menu item with the edited values
                        existingMenuItem.Name = model.Name;
                        existingMenuItem.Description = model.Description;
                        existingMenuItem.Price = model.Price;
                        existingMenuItem.Category = model.Category;

                        // Save the changes to the database
                        _dbContext.SaveChanges();

                        // Redirect to the menu items page or another appropriate page
                        return RedirectToAction("MenuItems");
                    }
                    else
                    {
                        // Handle the case where the menu item with the specified ID is not found
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception and return an appropriate error message
                    ModelState.AddModelError("", "An error occurred while updating the menu item: " + ex.Message);
                }
            }

            // If the model state is not valid, return the edit form with validation errors
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMenuItem(int id, string name, string description, decimal price, string category, IFormFile imageFile)
        {
            try
            {
                // Retrieve the existing menu item from the database
                var existingMenuItem = _dbContext.FoodItems.FirstOrDefault(item => item.Id == id);

                if (existingMenuItem != null)
                {
                    // Check if the new name already exists for the same vendor (case-insensitive)
                    bool nameExists = _dbContext.FoodItems
                        .Any(f => f.Name.ToLower() == name.ToLower() && f.VendorId == existingMenuItem.VendorId && f.Id != id);

                    if (nameExists)
                    {
                        return BadRequest("A food item with this name already exists.");
                    }

                    // Update the properties of the existing menu item with the edited values
                    existingMenuItem.Name = name;
                    existingMenuItem.Description = description;
                    existingMenuItem.Price = price;
                    existingMenuItem.Category = category;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "images/Foods/");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Update the image path in the database
                        existingMenuItem.FoodPicturePath = "/images/Foods/" + uniqueFileName;
                    }

                    // Save the changes to the database
                    _dbContext.SaveChanges();

                    return Ok(); // Return 200 OK status on successful update
                }
                else
                {
                    return NotFound(); // Return 404 Not Found if menu item with the specified ID is not found
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and return an appropriate error response
                return BadRequest($"An error occurred while updating the menu item: {ex.Message}");
            }
        }

        // Add this action method to handle deletion of individual menu items
        [HttpPost]
        public IActionResult DeleteMenuItem(int id)
        {
            try
            {
                var menuItemToDelete = _dbContext.FoodItems.FirstOrDefault(item => item.Id == id);
                if (menuItemToDelete != null)
                {
                    _dbContext.FoodItems.Remove(menuItemToDelete);
                    _dbContext.SaveChanges();
                    return Ok(); // Return 200 OK status on successful deletion
                }
                else
                {
                    return NotFound(); // Return 404 Not Found if menu item with the specified ID is not found
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and return an appropriate error response
                return BadRequest($"An error occurred while deleting the menu item: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PhotoGallery()
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not authenticated
            }

            // Retrieve all food items associated with the current vendor
            var vendorFoodItems = _dbContext.FoodItems.Where(item => item.VendorId == user.Id).ToList();

            // Pass the first food item to the view (you can adjust this logic based on your requirements)
            var firstFoodItem = vendorFoodItems.FirstOrDefault();

            return View(firstFoodItem);
        }

        [HttpPost]
        public IActionResult UploadPhoto(int itemId, IFormFile photo)
        {
            var menuItem = _dbContext.FoodItems.FirstOrDefault(item => item.Id == itemId);
            if (menuItem == null || photo == null || photo.Length <= 0)
            {
                return RedirectToAction("PhotoGallery", new { itemId = itemId });
            }

            try
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                menuItem.Photos.Add(new Photo { FilePath = "/images/Foods/" + uniqueFileName });
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("PhotoGallery", new { itemId = itemId });
        }

        [HttpPost]
        public IActionResult DeletePhoto(int itemId, int photoId)
        {
            var menuItem = _dbContext.FoodItems
                .Include(item => item.Photos)
                .FirstOrDefault(item => item.Id == itemId);

            if (menuItem != null)
            {
                var photoToRemove = menuItem.Photos.FirstOrDefault(photo => photo.Id == photoId);
                if (photoToRemove != null)
                {
                    menuItem.Photos.Remove(photoToRemove);
                    _dbContext.SaveChanges();

                    // Delete physical file if needed
                    // System.IO.File.Delete(photoToRemove.FilePath);
                }
            }

            return RedirectToAction("PhotoGallery", new { itemId = itemId });
        }

        // Method to get the Vendor ID
        private async Task<string> GetVendorId()
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User); // Get the user using UserManager if using ASP.NET Core Identity

            // Check if the user is authenticated
            if (user != null)
            {
                // Assuming VendorId is a property of the ApplicationUser class
                return user.Id; // Return the VendorId property of the user
            }
            else
            {
                // Handle the case where the user is not authenticated
                throw new Exception("User is not authenticated.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewReceivedQuotations()
        {
            var vendorId = _userManager.GetUserId(User);

            var quotations = await _dbContext.QuotationRequests
                .Where(q => q.VendorId == vendorId && q.Status == "Pending")
                .Include(q => q.QuotationFoodItems)
                .ToListAsync();

            return View(quotations);
        }

        [HttpPost]
        public async Task<IActionResult> HandleQuotation(int quotationId, string message, string action)
        {
            try
            {
                var quotation = await _dbContext.QuotationRequests.FirstOrDefaultAsync(q => q.Id == quotationId);

                if (quotation == null)
                {
                    return Json(new { success = false, error = "Quotation not found." });
                }

                if (action == "approve")
                {
                    // Approve the quotation
                    quotation.Status = "Approved";
                    quotation.VendorMessage = message;

                    var customer = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == quotation.CustomerId);
                    if (customer == null)
                    {
                        return Json(new { success = false, error = "Customer not found." });
                    }

                    var customerEmail = customer.Email;
                    var vendorName = User.Identity.Name;
                    var subject = $"Quotation Approved by {vendorName}";
                    var body = $"Dear {customer.UserName},\n\n" +
                               $"Your quotation request for {quotation.QuotationFoodItems.Count} items has been approved by {vendorName}.\n\n" +
                               $"Message from the vendor: {message}\n\n" +
                               $"Best regards,\n{vendorName}";

                    await SendQuotationEmailAsync(customerEmail, subject, body);
                }
                else if (action == "reject")
                {
                    // Reject the quotation
                    quotation.Status = "Rejected";
                    quotation.VendorMessage = message;

                    var customer = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == quotation.CustomerId);
                    if (customer == null)
                    {
                        return Json(new { success = false, error = "Customer not found." });
                    }

                    var customerEmail = customer.Email;
                    var vendorName = User.Identity.Name;
                    var subject = $"Quotation Rejected by {vendorName}";
                    var body = $"Dear {customer.UserName},\n\n" +
                               $"Your quotation request has been rejected by {vendorName}.\n\n" +
                               $"Message from the vendor: {message}\n\n" +
                               $"Best regards,\n{vendorName}";

                    await SendQuotationEmailAsync(customerEmail, subject, body);
                }

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ViewReceivedQuotations");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the quotation request.");
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
                throw new Exception("An error occurred while sending the email. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                // Log the error and handle it gracefully
                throw new Exception("An unexpected error occurred while sending the email. Please try again later.", ex);
            }
        }

    }
}
