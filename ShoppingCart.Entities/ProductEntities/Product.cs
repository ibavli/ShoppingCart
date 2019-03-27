using ShoppingCart.Entities.Cart;
using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.ProductEntities
{
    public class Product : BaseEntity
    {
        #region Constructor

        public Product()
        {
            ShoppingCartDetail = new List<ShoppingCartDetail>();
        }

        #endregion

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetail { get; set; }
    }
}
