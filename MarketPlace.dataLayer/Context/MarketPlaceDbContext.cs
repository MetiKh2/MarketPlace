using MarketPlace.dataLayer.DTOs.Site;
using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Entities.Contacts;
using MarketPlace.dataLayer.Entities.Discount;
using MarketPlace.dataLayer.Entities.Order;
using MarketPlace.dataLayer.Entities.Site;
using MarketPlace.dataLayer.Entities.Store;
using MarketPlace.dataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Context
{
   public class MarketPlaceDbContext:DbContext
    {
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options):base(options)
        {

        }
        #region Account
        public DbSet<User> Users{ get; set; }
        #endregion
        #region Site Setting
        public DbSet<SiteSetting> SiteSettings{ get; set; }
        public DbSet<Slider> Sliders{ get; set; }
        public DbSet<SiteBanner> SiteBanners{ get; set; }
        #endregion
        #region Contacts
        public DbSet<ContactUS> ContactsUs{ get; set; }
        public DbSet<Ticket> Tickets{ get; set; }
        public DbSet<TicketMessage> TicketMessages{ get; set; }
        #endregion
        #region Store
        public DbSet<Store> Stores{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSelectedCategory> ProductSelectedCategories{ get; set; }
        public DbSet<ProductColor> ProductColors{ get; set; }
        public DbSet<ProductGallery>ProductGalleries{ get; set; }
        public DbSet<ProductFeature>ProductFeatures{ get; set; }
        public DbSet<ProductDiscountUse>ProductDiscountUses{ get; set; }
        #endregion
        #region Order
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails{ get; set; }

        #endregion
        #region wallet
        public DbSet<StoreWallet> StoreWallets{ get; set; }
        #endregion
        #region discount
        public DbSet<ProductDiscount> ProductDiscounts{ get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.GetEntityTypes().SelectMany(p=>p.GetForeignKeys()))
            {
                item.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<SiteSetting>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<SiteBanner>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Slider>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ContactUS>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Ticket>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<TicketMessage>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Store>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductCategory>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductSelectedCategory>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductColor>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductGallery>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductFeature>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<OrderDetail>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Order>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<StoreWallet>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductDiscount>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductDiscountUse>().HasQueryFilter(p => !p.IsRemoved);

            base.OnModelCreating(modelBuilder);
        }
    }
}
