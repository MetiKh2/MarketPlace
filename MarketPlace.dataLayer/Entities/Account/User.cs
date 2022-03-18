using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Contacts;
using MarketPlace.dataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Account
{
    public class User : BaseEntity
    {
        #region props
        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string EmailActiveCode { get; set; }
        public bool IsEmailActive { get; set; }
        [Display(Name = "موبایل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string MobileActiveCode { get; set; }
        public bool IsMobileActive { get; set; }
        [Display(Name = "نام ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string LastName { get; set; }
        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }
        [Display(Name = "آواتار  ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Avatar { get; set; }
        public bool IsBlocked { get; set; }

        #endregion
        #region Relations
        public ICollection<ContactUS> ContactsUs { get; set; }
        public ICollection<Ticket> Tickets{ get; set; }
        public ICollection<TicketMessage> TicketMessages { get; set; }
        public ICollection<Store.Store> Stores{ get; set; }
        public ICollection<Order.Order> Orders { get; set; }
        public ICollection<ProductDiscountUse> ProductDiscountUses { get; set; }
        #endregion
    }
}
