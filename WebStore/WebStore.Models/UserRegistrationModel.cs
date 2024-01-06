using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Confirm Email Address")]
        [Compare(nameof(EmailAddress), ErrorMessage = "Emails are not the same. Please re-enter your email address.")]
        public string ConfirmEmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(80, ErrorMessage = "Your password must be between {2] and {1} characters.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not the same.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
