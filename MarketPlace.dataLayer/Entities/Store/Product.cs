using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Discount;
using MarketPlace.dataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class Product:BaseEntity
    {
        #region props
        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "تصویر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }
        public long SellerId { get; set; }
        [Display(Name = "شرح کوتاه")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShortDescription { get; set; }
        [Display(Name = "شرح")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public ProductAcceptanceState ProductAcceptanceState { get; set; }
        public string ProductAcceptOrRejectDescription { get; set; }
        [Display(Name ="درصد سایت")]
        public int SiteProfit { get; set; }
        #endregion
        #region rels
        [ForeignKey("SellerId")]
        public Store Store{ get; set; }
        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public ICollection<ProductColor>ProductColors{ get; set; }
        public ICollection<ProductGallery> ProductGalleries { get; set; }
        public ICollection<ProductFeature>ProductFeatures{ get; set; }
        public ICollection<OrderDetail> OrderDetails{ get; set; }
        public ICollection<ProductDiscount> ProductDiscounts{ get; set; }
        #endregion
    }

    public enum ProductAcceptanceState
    {
        [Display(Name ="درحال بررسی")]
        UnderProgress,
        [Display(Name ="پذیرفته شده")]
        Accepted,
        [Display(Name ="رد شده")]
        Rejected
    }
}
