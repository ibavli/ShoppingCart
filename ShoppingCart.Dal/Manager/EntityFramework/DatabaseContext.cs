using ShoppingCart.Entities.CategoryEntities;
using ShoppingCart.Entities.ProductEntities;
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
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Tabloların sonuna çoğul eki olan -s'yi eklemesin.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new CategoryMap());

        }
    }
}
