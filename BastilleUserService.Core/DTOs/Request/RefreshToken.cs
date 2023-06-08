using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.DTOs.Request
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string UserEmail { get; set; }
    }
}
