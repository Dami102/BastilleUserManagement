using System.ComponentModel.DataAnnotations;

namespace BastilleUserService.Core.DTOs.Request
{
    public class LoginDTO
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
    }
}
