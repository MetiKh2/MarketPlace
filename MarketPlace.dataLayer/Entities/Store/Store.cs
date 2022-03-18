using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class Store : BaseEntity
    {
        #region props
        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string StoreName { get; set; }
        public long UserId { get; set; }
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Phone { get; set; }
        [Display(Name = "موبایل")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Mobile { get; set; }
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Address { get; set; }
        [Display(Name = "لوگو")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Logo { get; set; }
        [Display(Name = "شرح")]
        public string Description { get; set; }
        [Display(Name = "شرح در ادمین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AdminDescription { get; set; }
        [Display(Name = "توضیحات وضعیت")]
        public string AcceptanceDescription { get; set; }
        public StoreAcceptanceState StoreAcceptanceState { get; set; }

        #endregion
        #region rels
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Wallet.StoreWallet> StoreWallets { get; set; }
        #endregion
    }
    public enum StoreAcceptanceState
    {
        [Display(Name ="در حال بررسی")]
        UnderProgress,
        [Display(Name ="تایید شده")]
        Accepted,
        [Display(Name ="رد شده")]
        Rejjected
    }
}
