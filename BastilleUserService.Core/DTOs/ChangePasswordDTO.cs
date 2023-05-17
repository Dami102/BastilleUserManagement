using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserService.Core.DTOs
{
    public class ChangePasswordDTO
    {
        [Required, DataType(DataType.Password), Display(Name = "Cuurent Password")]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = " NewPassword")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm new password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
