using ShoppingCart.Entities.CampaignEntities;
using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.CampaignAbs
{
    public interface ICampaignService
    {
        bool SaveCampaign(Campaign campaign = null, List<Category> categories = null);

        Campaign GetById(int Id);
    }
}
