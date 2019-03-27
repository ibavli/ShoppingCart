using ShoppingCart.Dal.Manager.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

            var products = db.Product.ToList();
            var categories = db.Category.ToList();
            var shoppingCarts = db.ShoppingCart.ToList();
            var shoppingCartDetails = db.ShoppingCartDetail.ToList();
            var campaign = db.Campaign.ToList();
            var campaignCategoryMapping = db.CampaignCategoryMapping.ToList();

        }
    }
}
