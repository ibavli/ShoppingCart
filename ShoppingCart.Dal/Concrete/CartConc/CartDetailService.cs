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
    public class CartDetailService : ICartDetailService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        public ShoppingCartDetail GetById(int Id)
        {
            return db.ShoppingCartDetail.FirstOrDefault(c => c.Id == Id);
        }

        public bool SaveShoppingCartDetail(List<ShoppingCartDetail> shoppingCartDetails)
        {
            if (shoppingCartDetails != null && shoppingCartDetails.Count > 0)
            {
                foreach (var cartDetail in shoppingCartDetails)
                {
                    db.ShoppingCartDetail.Add(cartDetail);
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
