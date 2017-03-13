using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BuyalotV1._0.DbConnection
{
    public class DataContext : DbContext
    {
        //connection to db
        public DataContext() :
            base("BuyalotContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }
        //Collections of the entities for the application
        public DbSet<CustomerModel> CustomerModelSet { get; set; }
        public DbSet<ProductModel> ProductModelSet { get; set; }
        public DbSet<ProductCategoryModel> ProductCategoryModelSet { get; set; }
        public DbSet<PaymentModel> PaymentModelSet { get; set; }
        public DbSet<OrderDetailModel> OrderDetailsModelSet { get; set; }
        public DbSet<OrderModel> OrderModelSet { get; set; }
        public DbSet<BillingModel> BillingModelSet { get; set; }
        public DbSet<AddressModel> AddressModelSet { get; set; }
        public DbSet<AdminModel> AdminModelSet { get; set; }
        public DbSet<SubscribeModel> SubscribeModelSet { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //public System.Data.Entity.DbSet<BuyalotV1._0.Models.InvoiceViewModel> InvoiceViewModels { get; set; }

        //public System.Data.Entity.DbSet<Buyalot.Models.CustomerInfoModel> CustomerInfoModels { get; set; }

        //public System.Data.Entity.DbSet<Buyalot..CustomerInfoModel> CustomerInfoModels { get; set; }

    }
}