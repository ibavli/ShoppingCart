using ShoppingCart.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.CartAbs
{
    public interface ICartDetailService
    {
        bool SaveShoppingCartDetail(List<ShoppingCartDetail> shoppingCartDetails);

        ShoppingCartDetail GetById(int Id);
    }
}
