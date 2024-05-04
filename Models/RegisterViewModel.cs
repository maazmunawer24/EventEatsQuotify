using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EventEatsQuotify.Models
{
    public class RegisterViewModel
    {
        // Properties for basic registration fields

        [Required(ErrorMessage = "Please enter your name.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters or spaces.")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SelectedRole { get; set; }
        public List<string> AvailableRoles { get; set; }

        // Properties specific to vendors
        [Display(Name = "CNIC Image Path")]
        [RequiredIf("SelectedRole", "Vendor", ErrorMessage = "CNIC Image Path is required for vendor role")]
        public string CNICImagePath { get; set; }

        [Display(Name = "Billing Image Path")]
        [RequiredIf("SelectedRole", "Vendor", ErrorMessage = "Billing Image Path is required for vendor role")]
        public string BillingImagePath { get; set; }

        [Display(Name = "CNIC Number")]
        [RequiredIf("SelectedRole", "Vendor", ErrorMessage = "CNIC Number is required for vendor role")]
        public string CNICNumber { get; set; }

        [Display(Name = "Shop Address")]
        [RequiredIf("SelectedRole", "Vendor", ErrorMessage = "Shop Address is required for vendor role")]
        public string ShopAddress { get; set; }

        [Display(Name = "CNIC Image")]
        [RequiredIf("SelectedRole", "Vendor", ErrorMessage = "CNIC image is required for vendor role")]
        public IFormFile CNICImageFile { get; set; }

        [Display(Name = "Billing Image")]
        [RequiredIf("SelectedRole", "Vendor", ErrorMessage = "Billing image is required for vendor role")]
        public IFormFile BillingImageFile { get; set; }

        public RegisterViewModel()
        {
            // Initialize AvailableRoles in the constructor
            AvailableRoles = new List<string>();

            // Provide a default value for SelectedRole
            SelectedRole = string.Empty;

            // Provide default values for non-nullable properties
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            CNICNumber = string.Empty;
            ShopAddress = string.Empty;
        }
    }

    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _desiredValue;

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            _propertyName = propertyName;
            _desiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var propertyInfo = instance.GetType().GetProperty(_propertyName);
            var propertyValue = propertyInfo.GetValue(instance);

            if (propertyValue != null && propertyValue.ToString() == _desiredValue.ToString())
            {
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
