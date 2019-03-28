using ShoppingCart.Entities.CampaignEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Mappings.CampaignMap
{
    public class CouponMap : BaseEntityMap<Coupon>
    {
        public CouponMap()
        {
            ToTable("Coupon");
        }
    }
}
