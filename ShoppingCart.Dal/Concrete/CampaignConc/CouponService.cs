using ShoppingCart.Dal.Abstract.CampaignAbs;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CampaignEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Concrete.CampaignConc
{
    public class CouponService : ICouponService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        public Coupon GetById(int Id)
        {
            return db.Coupon.FirstOrDefault(c => c.Id == Id);
        }

        public bool SaveCoupon(Coupon coupon)
        {
            if (coupon == null)
                return false;

            db.Coupon.Add(coupon);
            db.SaveChanges();
            return true;
        }
    }
}
