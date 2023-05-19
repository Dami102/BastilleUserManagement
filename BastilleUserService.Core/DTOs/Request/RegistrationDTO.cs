using System.ComponentModel.DataAnnotations;

namespace BastilleUserService.Core.DTOs.Request
{
    public class RegistrationDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "ComfirmPassword")]
        [Compare("Password", ErrorMessage = "Passwords Do not match")]
        public string ConfirmPassword { get; set; }
    }
}
