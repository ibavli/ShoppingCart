using ShoppingCart.Entities.CampaignEntities;
using ShoppingCart.Entities.Cart;
using ShoppingCart.Entities.CategoryEntities;
using ShoppingCart.Entities.ProductEntities;
using ShoppingCart.Mappings.CampaignMap;
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
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<CampaignCategoryMapping> CampaignCategoryMapping { get; set; }
        public DbSet<Coupon> Coupon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Tabloların sonuna çoğul eki olan -s'yi eklemesin.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ShoppingCartMap());
            modelBuilder.Configurations.Add(new ShoppingCartDetailMap());
            modelBuilder.Configurations.Add(new CampaignMap());
            modelBuilder.Configurations.Add(new CampaignCategoryMap());
            modelBuilder.Configurations.Add(new CouponMap());

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

            Category category4 = new Category()
            {
                Title = "Beyaz Eşya"
            };

            context.Category.Add(category4);

            Category category5 = new Category()
            {
                Title = "Mutfak Eşyaları"
            };

            context.Category.Add(category5);

            Category category6 = new Category()
            {
                Title = "Motosiklet"
            };

            context.Category.Add(category6);

            context.SaveChanges();

            #endregion

            #region Ürünler

            var telefonCat = context.Category.FirstOrDefault(c => c.Id == 1);
            var apple = context.Category.FirstOrDefault(c => c.Id == 3);
            var bilgisayar = context.Category.FirstOrDefault(c => c.Id == 2);
            var beyazesya = context.Category.FirstOrDefault(c => c.Id == 4);
            var mutfakesyalari = context.Category.FirstOrDefault(c => c.Id == 5);
            var motosiklet = context.Category.FirstOrDefault(c => c.Id == 6);

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
                Title = "IPhone 6s",
                Price = 2599,
                Category = apple
            };

            context.Product.Add(product3);

            Product product4 = new Product()
            {
                Title = "IPhone 7s",
                Price = 2999,
                Category = apple
            };

            context.Product.Add(product4);

            Product product5 = new Product()
            {
                Title = "IPhone 8s",
                Price = 3333,
                Category = apple
            };

            context.Product.Add(product5);

            Product product6 = new Product()
            {
                Title = "Vestel Venus",
                Price = 750,
                Category = telefonCat
            };

            context.Product.Add(product6);

            Product product7 = new Product()
            {
                Title = "Asus nw978x",
                Price = 2999,
                Category = bilgisayar
            };

            context.Product.Add(product7);

            Product product8 = new Product()
            {
                Title = "Lenovo l8xm8",
                Price = 3499,
                Category = bilgisayar
            };

            context.Product.Add(product8);

            Product product9 = new Product()
            {
                Title = "Çamaşır Makinası",
                Price = 4878,
                Category = beyazesya
            };

            context.Product.Add(product9);

            Product product10 = new Product()
            {
                Title = "HTC xyz",
                Price = 789,
                Category = telefonCat
            };

            context.Product.Add(product10);

            Product product11 = new Product()
            {
                Title = "Tencere",
                Price = 4000,
                Category = mutfakesyalari
            };

            context.Product.Add(product11);

            Product product12 = new Product()
            {
                Title = "Kask",
                Price = 1000,
                Category = motosiklet
            };

            context.Product.Add(product12);

            Product product13 = new Product()
            {
                Title = "Dizlik",
                Price = 1500,
                Category = motosiklet
            };

            context.Product.Add(product13);

            Product product14 = new Product()
            {
                Title = "Bot",
                Price = 2000,
                Category = motosiklet
            };

            context.Product.Add(product14);

            context.SaveChanges();

            #endregion

            #region Sepete ürün ekle

            #region Sepet1

            ShoppingCart.Entities.Cart.ShoppingCart cart1 = new Entities.Cart.ShoppingCart() { };

            context.ShoppingCart.Add(cart1);
            context.SaveChanges();

            var cart1db = context.ShoppingCart.FirstOrDefault(x => x.Id == 1);
            var cartIphone4s = context.Product.FirstOrDefault(x => x.Title == "IPhone 4s");
            var cartIphone5s = context.Product.FirstOrDefault(x => x.Title == "IPhone 5s");
            var cartIphone6s = context.Product.FirstOrDefault(x => x.Title == "IPhone 6s");
            var cartIphone7s = context.Product.FirstOrDefault(x => x.Title == "IPhone 7s");
            var cartIphone8s = context.Product.FirstOrDefault(x => x.Title == "IPhone 8s");
            var lenovoLaptop = context.Product.FirstOrDefault(x => x.Title == "Lenovo l8xm8");
            var htcTelefon = context.Product.FirstOrDefault(x => x.Title == "HTC xyz");
            ShoppingCartDetail cart1Product1 = new ShoppingCartDetail()
            {
                Product = cartIphone4s,
                Quantity = 2,
                ShoppingCart = cart1db
            };
            ShoppingCartDetail cart1Product2 = new ShoppingCartDetail()
            {
                Product = cartIphone5s,
                Quantity = 2,
                ShoppingCart = cart1db
            };
            ShoppingCartDetail cart1Product3 = new ShoppingCartDetail()
            {
                Product = cartIphone6s,
                Quantity = 3,
                ShoppingCart = cart1db
            };
            ShoppingCartDetail cart1Product4 = new ShoppingCartDetail()
            {
                Product = cartIphone7s,
                Quantity = 1,
                ShoppingCart = cart1db
            };
            ShoppingCartDetail cart1Product5 = new ShoppingCartDetail()
            {
                Product = cartIphone8s,
                Quantity = 1,
                ShoppingCart = cart1db
            };
            ShoppingCartDetail cart1Product6 = new ShoppingCartDetail()
            {
                Product = lenovoLaptop,
                Quantity = 1,
                ShoppingCart = cart1db
            };
            ShoppingCartDetail cart1Product7 = new ShoppingCartDetail()
            {
                Product = htcTelefon,
                Quantity = 1,
                ShoppingCart = cart1db
            };
            context.ShoppingCartDetail.Add(cart1Product1);
            context.ShoppingCartDetail.Add(cart1Product2);
            context.ShoppingCartDetail.Add(cart1Product3);
            context.ShoppingCartDetail.Add(cart1Product4);
            context.ShoppingCartDetail.Add(cart1Product5);
            context.ShoppingCartDetail.Add(cart1Product6);
            context.ShoppingCartDetail.Add(cart1Product7);
            context.SaveChanges();

            #endregion

            #region Sepet2

            ShoppingCart.Entities.Cart.ShoppingCart cart2 = new Entities.Cart.ShoppingCart() { };

            context.ShoppingCart.Add(cart2);
            context.SaveChanges();

            var cart2db = context.ShoppingCart.FirstOrDefault(x => x.Id == 2);
            var tencere = context.Product.FirstOrDefault(x => x.Title == "Tencere");
            ShoppingCartDetail cart2Product1 = new ShoppingCartDetail()
            {
                Product = tencere,
                Quantity = 2,
                ShoppingCart = cart2db
            };
           
            context.ShoppingCartDetail.Add(cart2Product1);
            context.SaveChanges();

            #endregion

            #region Sepet3

            ShoppingCart.Entities.Cart.ShoppingCart cart3 = new Entities.Cart.ShoppingCart() { };

            context.ShoppingCart.Add(cart3);
            context.SaveChanges();

            var cart3db = context.ShoppingCart.FirstOrDefault(x => x.Id == 3);
            var kask = context.Product.FirstOrDefault(x => x.Title == "Kask");
            var dizlik = context.Product.FirstOrDefault(x => x.Title == "Dizlik");
            var bot = context.Product.FirstOrDefault(x => x.Title == "Bot");

            ShoppingCartDetail cart3Product1 = new ShoppingCartDetail()
            {
                Product = kask,
                Quantity = 1,
                ShoppingCart = cart3db
            };
            ShoppingCartDetail cart3Product2 = new ShoppingCartDetail()
            {
                Product = dizlik,
                Quantity = 1,
                ShoppingCart = cart3db
            };
            ShoppingCartDetail cart3Product3 = new ShoppingCartDetail()
            {
                Product = bot,
                Quantity = 1,
                ShoppingCart = cart3db
            };

            context.ShoppingCartDetail.Add(cart3Product1);
            context.ShoppingCartDetail.Add(cart3Product2);
            context.ShoppingCartDetail.Add(cart3Product3);
            context.SaveChanges();

            #endregion

            #endregion

            #region İndirim Ekle

            #region İndirim 1

            Campaign campaign1 = new Campaign()
            {
                DiscountValue = 20.0,
                ProductCount = 3,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign1);
            context.SaveChanges();

            var appleCatForCampaign = context.Category.FirstOrDefault(c => c.Title == "Apple");

            var dbCampaign1 = context.Campaign.FirstOrDefault(c => c.Id == 1);

            CampaignCategoryMapping campaignCategoryMapping1 = new CampaignCategoryMapping()
            {
                Category = appleCatForCampaign,
                Campaign = dbCampaign1
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping1);

            #endregion

            #region İndirim 2

            Campaign campaign2 = new Campaign()
            {
                DiscountValue = 50.0,
                ProductCount = 5,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign2);
            context.SaveChanges();

            var dbCampaign2 = context.Campaign.FirstOrDefault(c => c.Id == 2);

            CampaignCategoryMapping campaignCategoryMapping2 = new CampaignCategoryMapping()
            {
                Category = appleCatForCampaign,
                Campaign = dbCampaign2
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping2);

            #endregion

            #region İndirim 3

            Campaign campaign3 = new Campaign()
            {
                DiscountValue = 5.0,
                ProductCount = 5,
                DiscountType = DiscountType.Amount
            };
            context.Campaign.Add(campaign3);
            context.SaveChanges();

            var dbCampaign3 = context.Campaign.FirstOrDefault(c => c.Id == 3);

            CampaignCategoryMapping campaignCategoryMapping3 = new CampaignCategoryMapping()
            {
                Category = appleCatForCampaign,
                Campaign = dbCampaign3
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping3);

            context.SaveChanges();

            #endregion

            #region İndirim 4

            Campaign campaign4 = new Campaign()
            {
                DiscountValue = 70,
                ProductCount = 25,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign4);
            context.SaveChanges();

            var dbCampaign4 = context.Campaign.FirstOrDefault(c => c.Id == 4);

            CampaignCategoryMapping campaignCategoryMapping4 = new CampaignCategoryMapping()
            {
                Category = appleCatForCampaign,
                Campaign = dbCampaign4
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping4);

            context.SaveChanges();

            #endregion

            #region İndirim 5

            var beyazEsyaCatForCampaign = context.Category.FirstOrDefault(c => c.Title == "Beyaz Eşya");

            Campaign campaign5 = new Campaign()
            {
                DiscountValue = 50,
                ProductCount = 1,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign5);
            context.SaveChanges();

            var dbCampaign5 = context.Campaign.FirstOrDefault(c => c.Id == 5);

            CampaignCategoryMapping campaignCategoryMapping5 = new CampaignCategoryMapping()
            {
                Category = beyazEsyaCatForCampaign,
                Campaign = dbCampaign5
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping5);

            context.SaveChanges();

            #endregion

            #region İndirim 6

            Campaign campaign6 = new Campaign()
            {
                DiscountValue = 51,
                ProductCount = 5,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign6);
            context.SaveChanges();

            var dbCampaign6 = context.Campaign.FirstOrDefault(c => c.Id == 6);

            var telefonCatForCampaign = context.Category.FirstOrDefault(c => c.Title == "Telefon");

            CampaignCategoryMapping campaignCategoryMapping6 = new CampaignCategoryMapping()
            {
                Category = telefonCatForCampaign,
                Campaign = dbCampaign6
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping6);

            context.SaveChanges();

            #endregion

            #region İndirim 7

            Campaign campaign7 = new Campaign()
            {
                DiscountValue = 10,
                ProductCount = 2,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign7);
            context.SaveChanges();

            var dbCampaign7 = context.Campaign.FirstOrDefault(c => c.Id == 7);

            var motosikletCatForCampaign = context.Category.FirstOrDefault(c => c.Title == "Motosiklet");

            CampaignCategoryMapping campaignCategoryMapping7 = new CampaignCategoryMapping()
            {
                Category = motosikletCatForCampaign,
                Campaign = dbCampaign7
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping7);

            context.SaveChanges();

            #endregion

            #region İndirim 8

            Campaign campaign8 = new Campaign()
            {
                DiscountValue = 20,
                ProductCount = 2,
                DiscountType = DiscountType.Rate
            };
            context.Campaign.Add(campaign8);
            context.SaveChanges();

            var dbCampaign8 = context.Campaign.FirstOrDefault(c => c.Id == 8);

            CampaignCategoryMapping campaignCategoryMapping8 = new CampaignCategoryMapping()
            {
                Category = motosikletCatForCampaign,
                Campaign = dbCampaign8
            };

            context.CampaignCategoryMapping.Add(campaignCategoryMapping8);

            context.SaveChanges();

            #endregion

            #endregion

            #region Kampanya Ekle

            Coupon coupon = new Coupon()
            {
                DiscountType = DiscountType.Rate,
                MinAmount = 2000,
                DiscountValue = 10
            };

            context.Coupon.Add(coupon);
            context.SaveChanges();

            #endregion

            #endregion
        }
    }
}
