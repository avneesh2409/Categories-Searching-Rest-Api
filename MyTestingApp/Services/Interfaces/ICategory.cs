using MyTestingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestingApp.Services.Interfaces
{
    public interface ICategory
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(Guid id);
        bool AddCategory(Category category);
        ICollection<Category> GetCategoriesInRange(int LowerBound,int UpperBound);
        IEnumerable<Category> GetCategoryWithProducts(Guid id);
    }
}
