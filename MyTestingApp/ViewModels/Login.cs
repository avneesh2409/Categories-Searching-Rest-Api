using System;
using System.ComponentModel.DataAnnotations;

namespace MyTestingApp.ViewModels
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password,ErrorMessage="Password invalid !!")]
        public string Password { get; set; }
    }
}