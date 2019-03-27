using ShoppingCart.Dal.Abstract.CategoryAbs;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Concrete.CategoryConc
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        public Category GetById(int Id)
        {
            return db.Category.FirstOrDefault(p => p.Id == Id);
        }

        public List<Category> GetCategories()
        {
            return db.Category.ToList();
        }

        public bool SaveCategory(Category category)
        {
            if (category != null)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return true;
            }

            return false;
        }
        
    }
}
