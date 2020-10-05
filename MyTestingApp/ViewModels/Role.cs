using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestingApp.ViewModels
{
    public class Role
    {
        [Required]
        public string Name { get; set; }
    }
}
