using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CampaignEntities;
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
            var campaigns = db.Campaign.ToList();
            var campaignCategoryMappings = db.CampaignCategoryMapping.ToList();

            WriteTitle("Oluşturulan Kategoriler");
            foreach (var category in categories)
            {
                System.Console.WriteLine($"Category Title : {category.Title}, Parant Category : {category.Parent?.Title ?? "Ana kategori yok"}");
            }

            WriteTitle("Oluşturulan Ürünler");
            foreach (var product in products)
            {
                System.Console.WriteLine($"Product Title : {product.Title}, Price : {product.Price}, Category : {product.Category.Title}, Parent Category : {product.Category.Parent?.Title ?? "Ana kategori yok"}");
            }

            WriteTitle("Oluşturulan Kampanyalar");
            foreach (var campaign in campaigns)
            {
                foreach (var camCats in campaign.Categories)
                {
                    System.Console.WriteLine($"Category : {camCats.Category.Title}, DiscountValue : {campaign.DiscountValue}, ProductCount : {campaign.ProductCount}, DiscountType : {campaign.DiscountType}");
                }
            }

            WriteTitle("Oluşturulan Sepetler");
            foreach (var shoppingCart in shoppingCarts)
            {
                System.Console.WriteLine($"Cart Id : {shoppingCart.Id} ");
                foreach (var cartDetail in shoppingCart.ShoppingCartDetail)
                {
                    System.Console.WriteLine($"CartId : {shoppingCart.Id}, Product : {cartDetail.Product.Title},  Quantity : {cartDetail.Quantity}, Price : {cartDetail.Product.Price}");
                }
                System.Console.WriteLine($"Sepetteki Ürünlerin sayısı : {shoppingCart.ShoppingCartDetail.Sum(c => c.Quantity)}");
                System.Console.WriteLine($"Sepetteki farklı Ürün sayısı : {shoppingCart.ShoppingCartDetail.Count()}");
                System.Console.WriteLine($"Sepetteki farklı Kategori sayısı : {shoppingCart.ShoppingCartDetail.Select(c => c.Product.CategoryId).Distinct().Count()}");
            }

            System.Console.ReadLine();
        }

        static void WriteTitle(string value)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("\n" + value.PadRight(System.Console.WindowWidth - 1));
            System.Console.ResetColor();
        }
    }
}
