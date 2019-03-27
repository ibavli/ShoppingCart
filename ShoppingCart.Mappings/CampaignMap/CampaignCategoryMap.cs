using ShoppingCart.Entities.CampaignEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Mappings.CampaignMap
{
    public class CampaignCategoryMap : BaseEntityMap<CampaignCategoryMapping>
    {
        public CampaignCategoryMap()
        {
            HasOptional(t => t.Campaign)
               .WithMany(t => t.Categories)
               .HasForeignKey(t => t.CampaignId).WillCascadeOnDelete(true);

            HasOptional(t => t.Category)
              .WithMany(t => t.Campaigns)
              .HasForeignKey(t => t.CategoryId).WillCascadeOnDelete(true);

            ToTable("CampaignCategoryMapping");
        }
    }
}
