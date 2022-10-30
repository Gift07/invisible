using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyApplicatioon.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = ("email is required"))]
        public string Email { get; set; }
        [Required(ErrorMessage = ("password is required"))]
        public string Password { get; set; }
    }
}