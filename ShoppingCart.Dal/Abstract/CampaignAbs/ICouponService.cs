using ShoppingCart.Entities.CampaignEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.CampaignAbs
{
    public interface ICouponService
    {
        bool SaveCoupon(Coupon coupon);

        Coupon GetById(int Id);
    }
}
