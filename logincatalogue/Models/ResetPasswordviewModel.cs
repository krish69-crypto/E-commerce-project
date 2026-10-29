using System.ComponentModel.DataAnnotations;

namespace logincatalogue.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string OTP { get; set; } = "";

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = "";

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
