using ShoppingCart.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.ProductAbs
{
    public interface IProductService
    {
        bool SaveProduct(Product product);

        Product GetById(int Id);

        List<Product> GetProducts();
    }
}
