using ShoppingCart.Dal.Abstract.CampaignAbs;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CampaignEntities;
using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Concrete.CampaignConc
{
    public class CampaignService : ICampaignService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        #region Utilities Methods

        public bool SaveCampaignsCategories(int campaignId = 0, List<Category> categories = null)
        {
            try
            {
                var campaign = db.Campaign.FirstOrDefault(c => c.Id == campaignId);
                foreach (var category in categories)
                {
                    var cat = db.Category.FirstOrDefault(c => c.Id == category.Id);
                    CampaignCategoryMapping campaignCategoryMapping = new CampaignCategoryMapping()
                    {
                        Category = cat,
                        Campaign = campaign
                    };
                    db.CampaignCategoryMapping.Add(campaignCategoryMapping);
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public Campaign GetById(int Id)
        {
            return db.Campaign.FirstOrDefault(c => c.Id == Id);
        }

        public bool SaveCampaign(Campaign campaign = null, List<Category> categories = null)
        {
            bool result = false;
            if(campaign != null && categories != null)
            {
                db.Campaign.Add(campaign);
                db.SaveChanges();
                result = true;

                int campaignId = db.Campaign.FirstOrDefault(c => c.CampaignTitle == campaign.CampaignTitle).Id;

                if (categories.Count > 0)
                    result = SaveCampaignsCategories(campaignId: campaignId, categories: categories);

            }
            return result;
        }
    }
}
