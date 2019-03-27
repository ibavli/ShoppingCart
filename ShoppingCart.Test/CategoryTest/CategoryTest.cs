using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Dal.Abstract.CategoryAbs;
using ShoppingCart.Dal.Concrete.CategoryConc;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Test.CategoryTest
{
    [TestClass]
    public class CategoryTest
    {
        private DatabaseContext _dbContext;
        private ICategoryService _categoryService;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContext = DatabaseContext.CreateDBWithSingleton();
            _categoryService = new CategoryService();
        }

        [TestMethod]
        public void AddCategory()
        {
            Category category = new Category()
            {
                Title = "test Category Title(Parent)"
            };

            bool result = _categoryService.SaveCategory(category);

            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void AddCategoryWithParentCategory()
        {
            Category parentCategory = _categoryService.GetById(1);

            Category category = new Category()
            {
                Title = "test Category Title",
                Parent = parentCategory
            };

            bool result = _categoryService.SaveCategory(category);

            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void GetCategory()
        {
            Category category = _categoryService.GetById(1);

            Assert.IsNotNull(category);
        }

        [TestMethod]
        public void GetCategories()
        {
            var categories = _categoryService.GetCategories();
            var count = categories.Count;
            Assert.AreNotEqual(0, count);
        }
    }
}
