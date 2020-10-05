using MyTestingApp.Models;
using MyTestingApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestingApp.Services.Repositories
{
    public class ProductRespository : IProduct
    {
        private readonly AppDbContext _context;

        public ProductRespository(AppDbContext context)
        {
            _context = context;
        }
        public bool AddProduct(Product product)
        {
            try
            {
                _context.products.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.products.ToList();
        }

        public Product GetProductById(int id)
        {
            Product product = _context.products.Find(id);
            return product;
        }

    }
}
