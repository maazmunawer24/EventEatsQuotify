using EventEatsQuotify.ContextDBConfig;
using EventEatsQuotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace EventEatsQuotify.Controllers
{
    public class VendorController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly EventEatsQuotifyDBContext _dbContext;

        public VendorController(IWebHostEnvironment environment, EventEatsQuotifyDBContext dbContext)
        {
            _environment = environment;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Vendor")]
        public IActionResult VendorDashboard()
        {
            // Actions related to the vendor role
            return View();
        }

        [Authorize(Roles = "Vendor")]
        public IActionResult MenuItems()
        {
            var menuItems = _dbContext.FoodItems.ToList();
            return View(menuItems);
        }

        [Authorize(Roles = "Vendor")]
        [HttpGet]
        public IActionResult UploadMenuItem(FoodItem model, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null && photo.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            photo.CopyTo(stream);
                        }
                        model.FoodPicturePath = "/images/" + uniqueFileName;
                    }

                    _dbContext.FoodItems.Add(model);
                    _dbContext.SaveChanges();

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

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public IActionResult EditMenuItem(FoodItem model)
        {
            // Logic for editing menu item
            return View();
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public IActionResult DeleteMenuItem(int id)
        {
            // Logic for deleting menu item
            return View();
        }
    }
}
