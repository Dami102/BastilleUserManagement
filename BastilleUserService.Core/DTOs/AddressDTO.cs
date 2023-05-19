using System.ComponentModel.DataAnnotations;

namespace BastilleUserService.Core.DTOs
{
    public class AddressDTO
    {
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string StreetNumber { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
