using MyTestingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestingApp.Services.Interfaces
{
    public interface IProduct
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        bool AddProduct(Product product);
    }
}
