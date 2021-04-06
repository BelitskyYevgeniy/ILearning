using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T_4.Models
{
    public class Register
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords is not identity!")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string Name { get; set; }
    }
}