using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.CartAbs
{
    public interface IShoppingCartService
    {
        bool SaveShoppingCart(ShoppingCart.Entities.Cart.ShoppingCart shoppingCart);

        ShoppingCart.Entities.Cart.ShoppingCart GetById(int Id);
    }
}
