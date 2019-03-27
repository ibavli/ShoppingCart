using ShoppingCart.Dal.Abstract.ProductAbs;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Concrete.ProductConc
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        public Product GetById(int Id)
        {
            return db.Product.FirstOrDefault(p => p.Id == Id);
        }

        public List<Product> GetProducts()
        {
            return db.Product.ToList();
        }

        public bool SaveProduct(Product product)
        {
            if (product != null)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
