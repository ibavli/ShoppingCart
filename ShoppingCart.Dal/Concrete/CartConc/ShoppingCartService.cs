using ShoppingCart.Dal.Abstract.CartAbs;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Concrete.CartConc
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        public Entities.Cart.ShoppingCart GetById(int Id)
        {
            return db.ShoppingCart.FirstOrDefault(c => c.Id == Id);
        }

        public bool SaveShoppingCart(Entities.Cart.ShoppingCart shoppingCart)
        {
            if(shoppingCart != null)
            {
                db.ShoppingCart.Add(shoppingCart);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
