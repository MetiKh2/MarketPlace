using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Order
{
    public class Order:BaseEntity
    {
        #region props
        public long UserId { get; set; }
        public DateTime? PermentDate { get; set; }
        public bool IsPay { get; set; }
        [Display(Name = "کد پیگیری")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TrackingCode { get; set; }
        [Display(Name = "شرح ادمین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AdminDescription { get; set; }
        #endregion
        #region rel
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public User User{ get; set; }
        #endregion
    }
}
