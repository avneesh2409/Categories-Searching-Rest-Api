using System;
using System.ComponentModel.DataAnnotations;

namespace MyTestingApp.ViewModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}