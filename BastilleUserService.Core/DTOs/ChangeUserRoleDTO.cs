using System.ComponentModel.DataAnnotations;

namespace BastilleUserService.Core.DTOs
{
    public class ChangeUserRoleDTO
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
