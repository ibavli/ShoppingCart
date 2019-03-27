﻿using ShoppingCart.Entities.Cart;
using ShoppingCart.Entities.CategoryEntities;
using ShoppingCart.Entities.ProductEntities;
using ShoppingCart.Mappings.CartMap;
using ShoppingCart.Mappings.CategoryMap;
using ShoppingCart.Mappings.ProductMap;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Manager.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        #region Singleton Design Pattern

        private static DatabaseContext databaseContext;
        static object _lock = new object();
        private DatabaseContext()
        {

        }
        public static DatabaseContext CreateDBWithSingleton()
        {
            lock (_lock)
            {
                if (databaseContext == null)
                {
                    databaseContext = new DatabaseContext();
                }
            }
            return databaseContext;
        }

        #endregion

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ShoppingCart.Entities.Cart.ShoppingCart> ShoppingCart { get; set; }
        public DbSet<ShoppingCart.Entities.Cart.ShoppingCartDetail> ShoppingCartDetail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Tabloların sonuna çoğul eki olan -s'yi eklemesin.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ShoppingCartMap());
            modelBuilder.Configurations.Add(new ShoppingCartDetailMap());

            Database.SetInitializer(new VeritabaniOlusurkenTablolaraBaslangicKayitlariEkleme());
        }
    }

    public class VeritabaniOlusurkenTablolaraBaslangicKayitlariEkleme : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            #region Veritabanı oluşurken örnek verileri kaydet

            #region Kategoriler

            Category category1 = new Category()
            {
                Title = "Telefon"
            };
            context.Category.Add(category1);

            Category category2 = new Category()
            {
                Title = "Bilgisayar"
            };

            context.Category.Add(category2);

            context.SaveChanges();


            var telefon = context.Category.FirstOrDefault(c => c.Id == 1);

            Category category3 = new Category()
            {
                Title = "Apple",
                Parent = telefon
            };

            context.Category.Add(category3);
            context.SaveChanges();

            #endregion

            #region Ürünler

            var apple = context.Category.FirstOrDefault(c => c.Id == 3);
            var bilgisayar = context.Category.FirstOrDefault(c => c.Id == 2);

            Product product1 = new Product()
            {
                Title = "IPhone 4s",
                Price = 999,
                Category = apple
            };

            context.Product.Add(product1);

            Product product2 = new Product()
            {
                Title = "IPhone 5s",
                Price = 1999,
                Category = apple
            };

            context.Product.Add(product2);

            Product product3 = new Product()
            {
                Title = "Asus nw978x",
                Price = 2999,
                Category = bilgisayar
            };

            context.Product.Add(product3);

            context.SaveChanges();

            #endregion

            #region Sepete ürün ekle

            ShoppingCart.Entities.Cart.ShoppingCart cart1 = new Entities.Cart.ShoppingCart() { };

            context.ShoppingCart.Add(cart1);
            context.SaveChanges();

            var cart1db = context.ShoppingCart.FirstOrDefault(x => x.Id == 1);
            var cartIphone4s = context.Product.FirstOrDefault(x => x.Title == "IPhone 4s");
            var cartIphone5s = context.Product.FirstOrDefault(x => x.Title == "IPhone 5s");
            ShoppingCartDetail cart1Product1 = new ShoppingCartDetail()
            {
                Product = cartIphone4s,
                Quantity = 2,
                ShoppingCart = cart1
            };
            ShoppingCartDetail cart1Product2 = new ShoppingCartDetail()
            {
                Product = cartIphone5s,
                Quantity = 2,
                ShoppingCart = cart1
            };
            context.ShoppingCartDetail.Add(cart1Product1);
            context.ShoppingCartDetail.Add(cart1Product2);
            context.SaveChanges();

            #endregion

            #endregion
        }
    }
}
