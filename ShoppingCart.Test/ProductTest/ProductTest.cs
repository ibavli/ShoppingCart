using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Dal.Abstract.ProductAbs;
using ShoppingCart.Dal.Concrete.ProductConc;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.ProductEntities;

namespace ShoppingCart.Test.ProductTest
{
    [TestClass]
    public class ProductTest
    {
        private DatabaseContext _dbContext;
        private IProductService _productService;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContext = DatabaseContext.CreateDBWithSingleton();
            _productService = new ProductService();
        }

        [TestMethod]
        public void AddProduct()
        {
            Product product = new Product()
            {
                Title = "product Title",
                Price = 588
            };

            bool result =_productService.SaveProduct(product);

            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void GetProduct()
        {
            Product product = _productService.GetById(1);

            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void GetProducts()
        {
            var products = _productService.GetProducts();
            var count = products.Count;
            Assert.AreNotEqual(0, count);
        }

    }
}
