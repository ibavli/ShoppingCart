using ShoppingCart.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Mappings.ProductMap
{
    public class ProductMap : BaseEntityMap<Product>
    {
        public ProductMap()
        {
            //one-to-many ilişkisi kuruldu.
            HasOptional(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);

            ToTable("Product");
        }
    }
}
