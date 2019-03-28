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
        /// Bu metot, kendisine verilen kampanyalar arasından sepet için maksimum indirim yapanı seçer.
        /// </summary>
        /// <param name="validCampaigns"></param>
        /// <returns></returns>
        private Campaign GetBestCampaign(List<Campaign> validCampaigns, Entities.Cart.ShoppingCart shoppingCart = null)
        {
            //Oran verilen kampanyalardan en iyi oranlıyı kampanyayı bul.
            var bestRateCampaign = validCampaigns.Where(c => c.DiscountType == DiscountType.Rate).OrderByDescending(c => c.DiscountValue).FirstOrDefault();

            //İndirim tutarı verilen kampanyalardan, tutarı en fazla olan kampanyayı bul.
            var bestAmountCampaign = validCampaigns.Where(c => c.DiscountType == DiscountType.Amount).OrderByDescending(c => c.DiscountValue).FirstOrDefault();

            //Eğer oran verilmiş bir kampanya yoksa, direkt indirim tutarı en iyi olanı dön. Çünkü karşılaştırmaya gerek kalmıyor.
            if (bestRateCampaign == null)
                return bestAmountCampaign;

            //Eğer indirim tutarı verilmiş bir kampanya yoksa, direkt en iyi verilmiş oranlı kampanyayı dön. Çünkü karşılaştırmaya gerek kalmıyor.
            if (bestAmountCampaign == null)
                return bestRateCampaign;

            //Fakat iki indirim türünden de mevcut ise, aralarından sepeti maksimum indirimi yapacak olanı seçelim.
            var categoryIds = bestRateCampaign.Categories.Select(x => x.CategoryId).ToList();
            var parentCategoryIds = db.Category.Where(c => categoryIds.Contains(c.ParentId)).Select(x => x.Id).ToList();

            var Ids = categoryIds;
            Ids.AddRange(parentCategoryIds.Cast<int?>());

            var totalProductCount = shoppingCart.ShoppingCartDetail.Where(c => Ids.Contains(c.Product.CategoryId)).Sum(x => x.Quantity);
            var totalProductPrice = shoppingCart.ShoppingCartDetail.Where(c => Ids.Contains(c.Product.CategoryId)).Sum(x => x.Quantity * x.Product.Price);

            var DiscountAmountWithRate = (((double)totalProductPrice * bestRateCampaign.DiscountValue) / 100);

            var DiscountAmountWithAmount = totalProductCount * bestAmountCampaign.DiscountValue;

            if (DiscountAmountWithRate >= DiscountAmountWithAmount)
                return bestRateCampaign;
            else
                return bestAmountCampaign;
        }

        /// <summary>
        /// Bu metot çeşitli elemelerden geçtikten sonra elde edilen en iyi kampanyaya göre sepetin yeni halini döndürür.
        /// </summary>
        /// <param name="bestCampaign"></param>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        private Entities.Cart.ShoppingCart PrepareNewShoppingCart(Campaign bestCampaign = null, Entities.Cart.ShoppingCart shoppingCart = null)
        {
            //Eğer sepete uygun kampanya yok ise sepetin kendisi döndürür.
            if (bestCampaign == null)
                return shoppingCart;

            var categoryIds = bestCampaign.Categories.Select(x => x.CategoryId).ToList();
            var parentCategoryIds = db.Category.Where(c => categoryIds.Contains(c.ParentId)).Select(x => x.Id).ToList();

            var Ids = categoryIds;
            Ids.AddRange(parentCategoryIds.Cast<int?>());

            switch (bestCampaign.DiscountType)
            {
                case DiscountType.Rate:     
                    //İndirim olanlar
                    var newShoppingCartDetailRate = shoppingCart.ShoppingCartDetail.Where(c => Ids.Contains(c.Product.CategoryId)).Select(x => { x.Product.Price = (((x.Product.Price) * (decimal)bestCampaign.DiscountValue) / 100); return x; }).ToList();
                    //İndirim olmayanlar
                    var newShoppingCartDetailRateNonDiscount = shoppingCart.ShoppingCartDetail.Where(c => !Ids.Contains(c.Product.CategoryId)).ToList();
                    newShoppingCartDetailRate.AddRange(newShoppingCartDetailRateNonDiscount);

                    shoppingCart.ShoppingCartDetail = newShoppingCartDetailRate;
                    return shoppingCart;
                case DiscountType.Amount:
                    //İndirim olanlar
                    var newShoppingCartDetailAmount = shoppingCart.ShoppingCartDetail.Where(c => Ids.Contains(c.Product.CategoryId)).Select(x => { x.Product.Price = (x.Product.Price - (decimal)bestCampaign.DiscountValue); return x; }).ToList();
                    //İndirim olmayanlar
                    var newShoppingCartDetailAmountNonDiscount = shoppingCart.ShoppingCartDetail.Where(c => !Ids.Contains(c.Product.CategoryId)).ToList();
                    newShoppingCartDetailAmount.AddRange(newShoppingCartDetailAmountNonDiscount);

                    shoppingCart.ShoppingCartDetail = newShoppingCartDetailAmount;
                    return shoppingCart;
                default:
                    return shoppingCart;
            }
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

            //ValidCampaigns metodundan, sepet için uygulanabilecek hiçbir kampanya geri dönmedi ise metoda mevcut sepeti geri döndürmesi için gönderiyoruz.
            if (validCampaigns.Count < 0)
                return PrepareNewShoppingCart(shoppingCart:shoppingCart);

            //Eğer sepete uygun bir adet kampanya varsa direkt ona uygun sepeti hazırlayıp dönüyoruz.
            if (validCampaigns.Count == 1)
                return PrepareNewShoppingCart(bestCampaign: validCampaigns.FirstOrDefault(), shoppingCart: shoppingCart);

            //Sepete uygun birden fazla sayıda kampanya mevcutsa, aralarındaki en iyisini buluyoruz.
            var getBestCampaign = GetBestCampaign(validCampaigns:validCampaigns, shoppingCart:shoppingCart);
                return PrepareNewShoppingCart(bestCampaign: getBestCampaign, shoppingCart: shoppingCart);
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
