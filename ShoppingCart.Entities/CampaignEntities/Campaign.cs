using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.CampaignEntities
{
    public class Campaign : BaseEntity
    {
        #region Constructor

        public Campaign()
        {
            Categories = new List<CampaignCategoryMapping>();
        }

        #endregion

        public string CampaignTitle { get; set; }

        public double DiscountValue { get; set; }

        public int ProductCount { get; set; }

        public int DiscountTypeId { get; set; }

        public DiscountType DiscountType { get; set; }

        public virtual ICollection<CampaignCategoryMapping> Categories { get; set; }
    }
}
