using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.CampaignEntities
{
    public class Coupon : BaseEntity
    {
        public decimal MinAmount { get; set; }

        public decimal DiscountValue { get; set; }

        public int DiscountTypeId { get; set; }

        public DiscountType DiscountType { get; set; }
    }
}
