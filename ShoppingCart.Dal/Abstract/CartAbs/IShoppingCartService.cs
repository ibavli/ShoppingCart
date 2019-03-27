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

        /// <summary>
        /// Sepete maksimum indirimi uygular.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        ShoppingCart.Entities.Cart.ShoppingCart ApplyDiscounts(ShoppingCart.Entities.Cart.ShoppingCart shoppingCart);
    }
}
