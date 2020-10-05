using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestingApp.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public Guid CreatedBy { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
