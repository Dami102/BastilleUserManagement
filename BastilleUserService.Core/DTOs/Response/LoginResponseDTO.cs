using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.DTOs.Response
{
    public class LoginResponseDTO
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
