using ShoppingCart.Entities.CampaignEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.CartAbs
{
    public interface IShoppingCartService
    {
        bool SaveShoppingCart(ShoppingCart.Entities.Cart.ShoppingCart shoppingCart);

        ShoppingCart.Entities.Cart.ShoppingCart GetById(int Id);

        /// <summary>
        /// Sepete maksimum indirimi uygular.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        ShoppingCart.Entities.Cart.ShoppingCart ApplyDiscounts(ShoppingCart.Entities.Cart.ShoppingCart shoppingCart);

        /// <summary>
        /// Verilen kuponu eğer koşullar uyuyorsa sepetin toplamına etkiler.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        decimal ApplyCoupon(Coupon coupon = null, decimal totalCartAmount = decimal.Zero);

        /// <summary>
        /// Verilen sepete uygun delivery cost değerini hesaplar.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        double DeliveryCostCalculator(ShoppingCart.Entities.Cart.ShoppingCart shoppingCart, double costPerDelivery = 1, double costPerProduct = 1, double fixedCost = 2.99);
    }
}
