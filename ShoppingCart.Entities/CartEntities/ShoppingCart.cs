using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.Cart
{
    public class ShoppingCart : BaseEntity
    {
        #region Constructor

        public ShoppingCart()
        {
            ShoppingCartDetail = new List<ShoppingCartDetail>();
        }

        #endregion

        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetail { get; set; }
    }
}
