using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Dal.Abstract.CartAbs;
using ShoppingCart.Dal.Abstract.ProductAbs;
using ShoppingCart.Dal.Concrete.CartConc;
using ShoppingCart.Dal.Concrete.ProductConc;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Test.CartTest
{
    [TestClass]
    public class CartTest
    {
        private DatabaseContext _dbContext;
        private IShoppingCartService _shoppingCartService;
        private ICartDetailService _cartDetailService;
        private IProductService _productService;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContext = DatabaseContext.CreateDBWithSingleton();
            _shoppingCartService = new ShoppingCartService();
            _cartDetailService = new CartDetailService();
            _productService = new ProductService();
        }

        [TestMethod]
        public void AddShoppingCart()
        {
            ShoppingCart.Entities.Cart.ShoppingCart cart = new Entities.Cart.ShoppingCart() { };

            bool result = _shoppingCartService.SaveShoppingCart(cart);

            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void AddShoppingCartDetail()
        {
            var cart = _shoppingCartService.GetById(1);
            var product1 = _productService.GetById(1);
            var product2 = _productService.GetById(2);

            ShoppingCartDetail cart1Product1 = new ShoppingCartDetail()
            {
                Product = product1,
                Quantity = 2,
                ShoppingCart = cart
            };
            ShoppingCartDetail cart1Product2 = new ShoppingCartDetail()
            {
                Product = product2,
                Quantity = 2,
                ShoppingCart = cart
            };
            List<ShoppingCartDetail> list = new List<ShoppingCartDetail>() { cart1Product1, cart1Product2 };
            bool result = _cartDetailService.SaveShoppingCartDetail(list);

            Assert.AreNotEqual(false, result);
        }
    }
}
