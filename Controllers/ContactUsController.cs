using Microsoft.AspNetCore.Mvc;
using EventEatsQuotify.ViewModels;
using EventEatsQuotify.Services;
using System;
using System.Threading.Tasks;
using EventEatsQuotify.Interfaces;

namespace EventEatsQuotify.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly string _adminEmail = ""; // Change this to the admin's email address
        private readonly IEmailSender _emailSender;

        public ContactUsController(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
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
                // Send email to admin using the EmailSender service
                await _emailSender.SendEmailAsync(_adminEmail, "New Contact Form Submission", $"Name: {model.Name}<br/>Email: {model.Email}<br/>Message: {model.Message}");

                // For demonstration purposes, we'll just display the submitted data
                return Content($"Name: {model.Name}, Email: {model.Email}, Message: {model.Message}");
            }
            // If the model is not valid, return the view with validation errors
            return View(model);
        }
    }
}
