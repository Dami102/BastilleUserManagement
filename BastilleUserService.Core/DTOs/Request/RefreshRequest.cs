using System.ComponentModel.DataAnnotations;

namespace BastilleUserService.Core.DTOs.Request
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}