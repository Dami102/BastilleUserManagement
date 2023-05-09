using Microsoft.AspNetCore.Identity;

namespace BastilleIUserLibrary.Domain.Model
{
    public class User: IdentityUser
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
