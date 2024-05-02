using Microsoft.AspNetCore.Mvc;
using EventEatsQuotify.ViewModels;
using EventEatsQuotify.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EventEatsQuotify.Interfaces;

namespace EventEatsQuotify.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly string _adminEmail = "maaz.munawer@outlook.com"; // Change this to the admin's email address
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactUsController> _logger;

        public ContactUsController(IEmailSender emailSender, ILogger<ContactUsController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emailSender.SendEmailAsync(_adminEmail, "New Contact Form Submission", $"Name: {model.Name}<br/>Email: {model.Email}<br/>Message: {model.Message}");

                    // For demonstration purposes, we'll just display the submitted data
                    TempData["ContactUSMessage"] = "Your Request has been submitted.";
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    _logger.LogError(ex, "Error sending email: No such host is known.");
                    ViewData["ErrorMessage"] = "An error occurred while sending the email(Check your internet connection). Please try again later.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending email.");
                    ViewData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                }
            }
            // Pass model back to the view along with any validation errors
            return View(model);
        }

    }
}
