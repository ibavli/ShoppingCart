using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Mappings.CartMap
{
    public class ShoppingCartMap : BaseEntityMap<ShoppingCart.Entities.Cart.ShoppingCart>
    {
        public ShoppingCartMap()
        {
            ToTable("ShoppingCart");
        }
    }
}
