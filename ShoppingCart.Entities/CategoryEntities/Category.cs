using ShoppingCart.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.CategoryEntities
{
    public class Category : BaseEntity
    {
        #region Constructor

        public Category()
        {
            Products = new List<Product>();
        }

        #endregion

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

