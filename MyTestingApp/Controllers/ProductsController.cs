using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTestingApp.Models;
using MyTestingApp.Services.Interfaces;

namespace MyTestingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductsController(IProduct product)
        {
            _product = product;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Getproducts()
        {
            return Ok(_product.GetProducts());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _product.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<bool> PostProduct(Product product)
        {
            var result = _product.AddProduct(product);
            return CreatedAtAction("GetProduct", result);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public ActionResult<Product> DeleteProduct(int id)
        {
            return Ok();

            //var product = await _product.FindAsync(id);
            //if (product == null)
            //{
            //    return NotFound();
            //}

            //_product.Remove(product);
            //await _product.SaveChangesAsync();

            //return product;
        }

        private bool ProductExists(int id)
        {
            return true;
            //return _product.Any(e => e.Id == id);
        }
    }
}
