using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Dal.Abstract.CampaignAbs;
using ShoppingCart.Dal.Abstract.CategoryAbs;
using ShoppingCart.Dal.Concrete.CampaignConc;
using ShoppingCart.Dal.Concrete.CategoryConc;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CampaignEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Test.CampaignTest
{
    [TestClass]
    public class CampaignTest
    {
        private DatabaseContext _dbContext;
        private ICampaignService _campaignService;
        private ICategoryService _categoryService;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContext = DatabaseContext.CreateDBWithSingleton();
            _campaignService = new CampaignService();
            _categoryService = new CategoryService();
        }

        [TestMethod]
        public void AddShoppingCart()
        {
            var categories = _categoryService.GetCategories(new List<int> { 3 });
            Campaign campaign = new Campaign()
            {
                DiscountValue = 20.0,
                ProductCount = 3,
                DiscountType = DiscountType.Rate
            };
            var result = _campaignService.SaveCampaign(campaign: campaign, categories: categories);

            Assert.AreNotEqual(false, result);
        }

        [TestMethod]
        public void GetCampaign()
        {
            Campaign campaign = _campaignService.GetById(1);

            Assert.IsNotNull(campaign);
        }
    }
}
