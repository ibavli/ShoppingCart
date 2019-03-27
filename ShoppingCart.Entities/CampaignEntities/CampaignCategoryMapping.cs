using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.CampaignEntities
{
    public class CampaignCategoryMapping : BaseEntity
    {
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int? CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }
    }
}
