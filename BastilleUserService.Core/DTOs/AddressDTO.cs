using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.DTOs
{
    public class AddressDTO
    {
        public string City { get; set; }

        public string State { get; set; }

        public string StreetNumber { get; set; }

        public string Country { get; set; }
    }
}
