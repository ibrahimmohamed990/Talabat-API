using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models
{
    public class RegisterVM
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one digit")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one digit")]
        public string ConfirmPassword { get; set; }

    }
}
