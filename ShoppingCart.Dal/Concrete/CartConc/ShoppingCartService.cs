using ShoppingCart.Dal.Abstract.CartAbs;
using ShoppingCart.Dal.Manager.EntityFramework;
using ShoppingCart.Entities.CampaignEntities;
using ShoppingCart.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Concrete.CartConc
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DatabaseContext db = DatabaseContext.CreateDBWithSingleton();

        #region Utilities Methods

        /// <summary>
        /// Bu metot, kendisine verilen sepetteki mevcut tüm kampanyaları analiz eder ve kampanyalardan uygun olanların listesini geri döner.
        /// </summary>
        /// <param name="allCampaigns"></param>
        /// <returns></returns>
        private List<Campaign> ValidCampaigns(Entities.Cart.ShoppingCart shoppingCart)
        {
            //Kampanyaların listesini alalım.
            var allCampaigns = db.Campaign.ToList();

            //Sepetteki ürünleri kategori ve ürünlerin miktarına göre group by yapalım.
            var CategoryProductGroupBy = shoppingCart.ShoppingCartDetail.GroupBy(c => c.Product.Category).Select(g => new
            {
                Category = g.FirstOrDefault().Product.Category,
                ProductQuantity = g.Sum(c => c.Quantity)
            }).ToList();

            //Sepetteki ürünleri kategorilerin parent kategorilerini de analiz ederek group by yapalım.
            var ParentCategoryProductGroupBy = shoppingCart.ShoppingCartDetail.Where(p => p.Product.Category.Parent != null).GroupBy(c => c.Product.Category.Parent).Select(g => new
            {
                Category = g.FirstOrDefault().Product.Category.Parent,
                ProductQuantity = g.Sum(c => c.Quantity)
            }).ToList();


            var catIds = CategoryProductGroupBy.Select(c => c.Category.Id).ToList();
            var parentCatIds = ParentCategoryProductGroupBy.Select(c => c.Category.Id).ToList();

            //Kampanyaların içerisinde, kategori ve sepetteki bulunma miktarı uygun düşenleri analiz edelim.
            var returnModel = allCampaigns.Where(c => c.Categories.Any(x => catIds.Contains((int)x.CategoryId) && x.Campaign.ProductCount < CategoryProductGroupBy.Where(y => y.Category.Id == x.CategoryId).FirstOrDefault().ProductQuantity)).ToList();

            var parentCatModel = allCampaigns.Where(c => c.Categories.Any(x => parentCatIds.Contains((int)x.CategoryId) && x.Campaign.ProductCount < ParentCategoryProductGroupBy.Where(y => y.Category.Id == x.CategoryId).FirstOrDefault().ProductQuantity)).ToList();

            //Parent kategorideki kampanyaları da ekleyelim.
            returnModel.AddRange(parentCatModel);

            return returnModel;
        }

        /// <summary>
        /// Sepete uygun kampanya/kampanyaları metoda gönderip, her bir ürün için, o ürüne uygulanacak maksimum indirimi uygulatıp, yeni sepeti dönüyoruz.
        /// </summary>
        /// <param name="bestCampaign"></param>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        private Entities.Cart.ShoppingCart PrepareNewShoppingCart(List<Campaign> validCampaigns = null, Entities.Cart.ShoppingCart shoppingCart = null)
        {
            shoppingCart.ShoppingCartDetail.Select(x => { x.Product.Price = GetDiscountPrice(validCampaigns: validCampaigns, product: x.Product); return x; }).ToList();
            return shoppingCart;
        }

        /// <summary>
        /// Kendisine verilen ürünü, o ürünün kategori/ana kategorisine tanımlanmış indirimlerden en fazla indirim yapanı bulup, indirimi yapar.
        /// </summary>
        /// <param name="validCampaigns"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private decimal GetDiscountPrice(List<Campaign> validCampaigns = null, ShoppingCart.Entities.ProductEntities.Product product = null)
        {
            //O ürüne uygulanabilecek kampanyalardan Rate için en iyi olanı.
            var campaingsRate = validCampaigns.Where(c => c.DiscountType == DiscountType.Rate && c.Categories.Any(x => x.CategoryId == product.Category?.Id || x.CategoryId == product.Category?.Parent?.Id)).OrderByDescending(a => a.DiscountValue).FirstOrDefault();

            //O ürüne uygulanabilecek kampanyalardan Amount için en iyi olanı.
            var campaingsAmount = validCampaigns.Where(c => c.DiscountType == DiscountType.Amount && c.Categories.Any(x => x.CategoryId == product.Category?.Id || x.CategoryId == product.Category?.Parent?.Id)).OrderByDescending(a => a.DiscountValue).FirstOrDefault();

            //Eğer bu ürüne ait hiçbir uyumlu kampanya yoksa
            if (campaingsRate == null && campaingsAmount == null)
                return product.Price;

            //Eğer oran verilmiş bir kampanya yoksa, direkt amount için indirimli fiyatı hesapla.
            if (campaingsRate == null && campaingsAmount != null)
                return (product.Price - (decimal)campaingsAmount.DiscountValue);

            //Eğer indirim tutarı verilmiş bir kampanya yoksa, direkt rate değeri için indirimli fiyatı hesapla.
            if (campaingsAmount == null && campaingsRate != null)
                return (product.Price - (((product.Price) * (decimal)campaingsRate.DiscountValue) / 100));

            //Eğer ikisinden de mevcutsa bu ürüne daha fazla indirim yapmış olanı tespit et ve o fiyatı dön.
            var discountWithAmount = (product.Price - (decimal)campaingsAmount.DiscountValue);
            var discountWithRate = (product.Price - (((product.Price) * (decimal)campaingsRate.DiscountValue) / 100));

            if (discountWithAmount > discountWithRate)
                return discountWithRate;
            else
                return discountWithAmount;
        }

        /// <summary>
        /// Bu metot kendisine verilen kuponun türüne göre yeni sepet toplamını hesaplar.
        /// </summary>
        /// <param name="discountType"></param>
        /// <param name="totalCartAmount"></param>
        /// <returns></returns>
        private decimal CalculateNewCartAmount(Coupon coupon, decimal totalCartAmount)
        {
            switch (coupon.DiscountType)
            {
                case DiscountType.Rate:
                    return (totalCartAmount - (totalCartAmount * coupon.DiscountValue) / 100);
                case DiscountType.Amount:
                    return (totalCartAmount - coupon.DiscountValue);
                default:
                    return totalCartAmount;
            }
        }

        #endregion

        /// <summary>
        /// Sepete maksimum indirimi uygular
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public Entities.Cart.ShoppingCart ApplyDiscounts(Entities.Cart.ShoppingCart shoppingCart)
        {
            //Bu metot, kendisine verilen sepetteki mevcut tüm kampanyaları analiz eder ve kampanyalardan uygun olanların listesini geri döner.
            var validCampaigns = ValidCampaigns(shoppingCart: shoppingCart);

            //ValidCampaigns metodundan, sepet için uygulanabilecek hiçbir kampanya geri dönmedi ise mevcut sepeti dönüyoruz.
            if (validCampaigns.Count == 0)
                return shoppingCart;

            //Sepete uygun kampanya/kampanyaları metoda gönderip, her bir ürün için, o ürüne uygulanacak maksimum indirimi uygulatıp, yeni sepeti dönüyoruz.
            return PrepareNewShoppingCart(validCampaigns: validCampaigns, shoppingCart: shoppingCart);
        }


        /// <summary>
        /// Verilen kuponu eğer koşullar uyuyorsa sepetin toplamına etkiler.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public decimal ApplyCoupon(Coupon coupon = null, decimal totalCartAmount = decimal.Zero)
        {
            //Kupon yok ise, direkt sepet toplamını dön.
            if (coupon == null)
                return totalCartAmount;

            //Kupona tanımlananan minimum sepet toplamı, sepet toplamından büyük ise kupon uygulanamaz.
            if (coupon.MinAmount > totalCartAmount)
                return totalCartAmount;

            //Bu metot kendisine verilen kuponun türüne göre yeni sepet toplamını hesaplar.
            totalCartAmount = CalculateNewCartAmount(coupon: coupon, totalCartAmount: totalCartAmount);

            return totalCartAmount;
        }


        /// <summary>
        /// Verilen sepete uygun delivery cost değerini hesaplar.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public double DeliveryCostCalculator(Entities.Cart.ShoppingCart shoppingCart, double costPerDelivery = 1, double costPerProduct = 1, double fixedCost = 2.99)
        {
            //Sepetteki farklı kategorilerin sayısı
            var numberOfDeliveries = shoppingCart.ShoppingCartDetail.Select(c => c.Product.CategoryId).Distinct().Count();

            //Sepetteki farklı ürünlerin sayısı
            var numberOfProducts = shoppingCart.ShoppingCartDetail.Count();

            return ((costPerDelivery * numberOfDeliveries) + (costPerProduct * numberOfProducts) + fixedCost);
        }


        public Entities.Cart.ShoppingCart GetById(int Id)
        {
            return db.ShoppingCart.FirstOrDefault(c => c.Id == Id);
        }

        public bool SaveShoppingCart(Entities.Cart.ShoppingCart shoppingCart)
        {
            if (shoppingCart != null)
            {
                db.ShoppingCart.Add(shoppingCart);
                db.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
