using System.ComponentModel.DataAnnotations;
namespace EventEatsQuotify.Models
{
    public class RegisterViewModel
    {
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // New property for selecting role during registration
        [Required(ErrorMessage = "Please select a role")]
        public string SelectedRole { get; set; }

        // List of roles for the dropdown
        public List<string> AvailableRoles { get; set; }

        public RegisterViewModel()
        {
            // Initialize AvailableRoles in the constructor
            AvailableRoles = new List<string>();

            // Provide a default value for SelectedRole
            SelectedRole = string.Empty; // You can set this to a default role if needed

            // Provide default values for non-nullable properties
            Name = string.Empty;
            Address = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}