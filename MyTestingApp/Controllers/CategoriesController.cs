using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTestingApp.Models;
using MyTestingApp.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyTestingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoriesController(ICategory category)
        {
            _category = category;
        }
        // GET: api/categories
        [HttpGet]
        public IEnumerable<Category> Get()
        {

            return _category.GetCategories();
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public Category Get(Guid id)
        {
            Category category = _category.GetCategoryById(id);
            return category;
        }
        [HttpGet("range/{lower}/{upper}")]
        public ObjectResult CategoriesInRange(int lower, int upper) {
            var result = _category.GetCategoriesInRange(lower, upper);
            return new ObjectResult(new { count=result.Count,categories=result});
        }
        [HttpGet("categorywithproducts/{id}")]
        public ObjectResult CategoryWithProducts(Guid id)
        {
            var result = _category.GetCategoryWithProducts(id);
            return new ObjectResult(new { categories = result });
        }

        // POST api/categories
        [HttpPost]
        public bool Post(Category category)
        {
            bool result = _category.AddCategory(category);
            return result;
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
