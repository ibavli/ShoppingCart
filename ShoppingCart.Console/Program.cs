﻿using Autofac;
using ShoppingCart.Dal.Abstract.CartAbs;
using ShoppingCart.Dal.Concrete.CartConc;
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
            #region Kontrol Amaçlı

            DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

            var products = db.Product.ToList();
            var categories = db.Category.ToList();
            var shoppingCarts = db.ShoppingCart.ToList();
            var shoppingCartDetails = db.ShoppingCartDetail.ToList();
            var campaigns = db.Campaign.ToList();
            var campaignCategoryMappings = db.CampaignCategoryMapping.ToList();

            #endregion

            var container = BuildContainer();
            var _shoppingCartService = container.Resolve<IShoppingCartService>();

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
            var totalCartAmount = decimal.Zero;
            foreach (var shoppingCart in shoppingCarts)
            {
                System.Console.WriteLine($"Cart Id : {shoppingCart.Id} ");
                foreach (var cartDetail in shoppingCart.ShoppingCartDetail)
                {
                    System.Console.WriteLine($"CartId : {shoppingCart.Id}, Product : {cartDetail.Product.Title},  Quantity : {cartDetail.Quantity}, Price : {cartDetail.Product.Price}, Total = {cartDetail.Quantity * cartDetail.Product.Price}");
                    totalCartAmount += (cartDetail.Quantity * cartDetail.Product.Price);
                }
                System.Console.WriteLine($"Sepetteki Ürünlerin sayısı : {shoppingCart.ShoppingCartDetail.Sum(c => c.Quantity)}");
                System.Console.WriteLine($"Sepetteki farklı Ürün sayısı : {shoppingCart.ShoppingCartDetail.Count()}");
                System.Console.WriteLine($"Sepetteki farklı Kategori sayısı : {shoppingCart.ShoppingCartDetail.Select(c => c.Product.CategoryId).Distinct().Count()}");
                System.Console.WriteLine($"Sepet toplam tutarı : {totalCartAmount}");

                totalCartAmount = 0;
                WriteTitle("Sepetin İndirim Uygulanmış Hali");
                var shoppingCartWithDiscount = _shoppingCartService.ApplyDiscounts(shoppingCart);
                foreach (var _cartDetail in shoppingCartWithDiscount.ShoppingCartDetail)
                {
                    System.Console.WriteLine($"CartId : {shoppingCartWithDiscount.Id}, Product : {_cartDetail.Product.Title},  Quantity : {_cartDetail.Quantity}, Price : {_cartDetail.Product.Price}, Total = {_cartDetail.Quantity * _cartDetail.Product.Price}");
                    totalCartAmount += (_cartDetail.Quantity * _cartDetail.Product.Price);
                }
                System.Console.WriteLine($"Sepetteki Ürünlerin sayısı : {shoppingCart.ShoppingCartDetail.Sum(c => c.Quantity)}");
                System.Console.WriteLine($"Sepetteki farklı Ürün sayısı : {shoppingCart.ShoppingCartDetail.Count()}");
                System.Console.WriteLine($"Sepetteki farklı Kategori sayısı : {shoppingCart.ShoppingCartDetail.Select(c => c.Product.CategoryId).Distinct().Count()}");
                System.Console.WriteLine($"Sepet toplam tutarı : {totalCartAmount}");
                totalCartAmount = 0;

            }


            System.Console.ReadLine();
        }

        static void WriteTitle(string value)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("\n" + value.PadRight(System.Console.WindowWidth - 1));
            System.Console.ResetColor();
        }


        #region Autofac DI ()

        /// <summary>
        /// Bu metod Autofac ile dependency injection yapılmak için yazıldı. Daha önceden dependency injection için Ninject ve Unity kullanmıştım. Autofac'i de ilk kez burada kullanıyorum.
        /// </summary>
        /// <returns></returns>
        static private Autofac.IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ShoppingCartService>()
                   .As<IShoppingCartService>()
                   .InstancePerDependency();
     
            return builder.Build();
        }

        #endregion

    }
}
