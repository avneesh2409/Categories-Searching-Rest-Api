using Microsoft.EntityFrameworkCore;
using MyTestingApp.Models;
using MyTestingApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestingApp.Services.Repositories
{
    public class CategoryRepository : ICategory
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool AddCategory(Category category)
        {
            try
            {
                _context.categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;    
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.categories.ToList();
        }

        public ICollection<Category> GetCategoriesInRange(int LowerBound, int UpperBound)
        {
            var pagedProductQuery = _context.categories.Skip(LowerBound).Take(UpperBound).OrderBy(e=>e.Name).ToList();
            return pagedProductQuery;
        }

        public Category GetCategoryById(Guid id)
        {
            Category category = _context.categories.Find(id);
            return category;
        }

        public IEnumerable<Category> GetCategoryWithProducts(Guid id)
        {
            try {
                var result = _context.categories.Include(e => e.Products).Where(e => e.Id == id).ToList();
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
