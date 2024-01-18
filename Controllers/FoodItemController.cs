using EventEatsQuotify.ContextDBConfig;
using EventEatsQuotify.Models;
using EventEatsQuotify.Services;
using EventEatsQuotify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class FoodItemController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EventEatsQuotifyDBContext _context;
    private readonly VendorService _vendorService;
    private readonly ICompositeViewEngine _viewEngine;

    public FoodItemController(
        UserManager<ApplicationUser> userManager,
        EventEatsQuotifyDBContext context,
        VendorService vendorService,
        ICompositeViewEngine viewEngine)
    {
        _userManager = userManager;
        _context = context;
        _vendorService = vendorService;
        _viewEngine = viewEngine;
    }

    // GET: /FoodItem
    public async Task<IActionResult> Index()
    {
        var viewModel = new FoodItemViewModel
        {
            FoodItems = await GetFoodItemsAsync(),
            Vendors = await _vendorService.GetAllVendors() // Retrieve vendors using VendorService
        };

        return View(viewModel);
    }

    // POST: /FoodItem/Quotation
    [HttpPost]
    public IActionResult Quotation(FoodItemViewModel viewModel)
    {
        // Process the customer's quotation request
        // You can access the selected food items and quantities from viewModel.SelectedFoodItems and viewModel.SelectedQuantities

        // For now, let's return a dummy view with the selected items and quantities
        return View(viewModel);
    }

    // GET: /FoodItem/Upload
    [HttpGet]
    [Authorize(Roles = "Vendor")]
    public IActionResult Upload()
    {
        return View();
    }

    // POST: /FoodItem/Upload
    [HttpPost]
    [Authorize(Roles = "Vendor")]
    public async Task<IActionResult> Upload(VendorFoodItemUploadModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Ensure that each food item is associated with the current vendor
            string currentUserId = _userManager.GetUserId(User);
            viewModel.FoodItems.ForEach(foodItem => foodItem.VendorId = currentUserId);

            // Save each food item to the database
            _context.FoodItems.AddRange(viewModel.FoodItems);
            await _context.SaveChangesAsync();

            // Return the updated HTML for the grid using a partial view
            var updatedFoodItems = await GetFoodItemsAsync();

            // Render the partial view "_FoodItemsGrid" and pass the model directly
            return PartialView("_FoodItemsGrid", updatedFoodItems);
        }

        // If the model state is not valid, return to the upload view with errors
        //viewModel.Vendors = await _vendorService.GetAllVendors(); // You may need to update vendors based on your logic
        return View(viewModel);
    }

    // Helper method to render a partial view to string
    private async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            viewName = this.ControllerContext.ActionDescriptor.ActionName;
        }
        ViewData.Model = model;

        using (var sw = new StringWriter())
        {
            var viewEngineResult = _viewEngine.FindView(ControllerContext, viewName, false);

            var viewContext = new ViewContext(
                ControllerContext,
                viewEngineResult.View,
                ViewData,
                TempData,
                sw,
                new HtmlHelperOptions()
            );

            await viewEngineResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }


    // GET: /FoodItem/Edit/1
    [HttpGet]
    [Authorize(Roles = "Vendor")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var foodItem = await _context.FoodItems.FindAsync(id);

        if (foodItem == null)
        {
            return NotFound();
        }

        return View(foodItem);
    }

    // POST: /FoodItem/Edit/1
    [HttpPost]
    [Authorize(Roles = "Vendor")]
    public async Task<IActionResult> Edit(int id, FoodItem foodItem)
    {
        if (id != foodItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Update the food item in the database
                _context.Entry(foodItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redirect to the upload view after successful edit
            return RedirectToAction("Upload");
        }

        // If the model state is not valid, return to the edit view with errors
        return View(foodItem);
    }

    // POST: /FoodItem/Delete/1
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Vendor")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var foodItem = await _context.FoodItems.FindAsync(id);

        if (foodItem == null)
        {
            return NotFound();
        }

        // Remove the food item from the database
        _context.FoodItems.Remove(foodItem);
        await _context.SaveChangesAsync();

        // Redirect to the upload view after successful deletion
        return RedirectToAction("Upload");
    }

    private async Task<List<FoodItem>> GetFoodItemsAsync()
    {
        // Retrieve food items from the database
        return await _context.FoodItems.ToListAsync();
    }

    private bool FoodItemExists(int id)
    {
        return _context.FoodItems.Any(e => e.Id == id);
    }
}
