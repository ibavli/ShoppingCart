using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Mappings.CartMap
{
    public class ShoppingCartDetailMap : BaseEntityMap<ShoppingCart.Entities.Cart.ShoppingCartDetail>
    {
        public ShoppingCartDetailMap()
        {
            //one-to-many ilişkisi kuruldu.
            HasRequired(x => x.ShoppingCart)
                .WithMany(x => x.ShoppingCartDetail)
                .HasForeignKey(x => x.ShoppingCartId).WillCascadeOnDelete(false);

            //one-to-many ilişkisi kuruldu.
            HasRequired(x => x.Product)
                .WithMany(x => x.ShoppingCartDetail)
                .HasForeignKey(x => x.ProductId).WillCascadeOnDelete(false);

            ToTable("ShoppingCartDetail");
        }
    }
}
