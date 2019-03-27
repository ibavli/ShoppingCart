using ShoppingCart.Entities.CampaignEntities;
using ShoppingCart.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.CategoryEntities
{
    public class Category : BaseEntity
    {
        #region Constructor

        public Category()
        {
            Products = new List<Product>();
            Campaigns = new List<CampaignCategoryMapping>();
        }

        #endregion

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public ICollection<Product> Products { get; set; }

        public virtual ICollection<CampaignCategoryMapping> Campaigns { get; set; }

    }
}

