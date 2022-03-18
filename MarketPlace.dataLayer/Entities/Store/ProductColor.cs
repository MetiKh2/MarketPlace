using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class ProductColor:BaseEntity
    {
        #region props
        [Display(Name = "نام رنگ")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")] 
        public string ColorName { get; set; }
        [Display(Name = "کد رنگ")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ColorCode { get; set; }
        public int Price { get; set; }
        public long ProductId { get; set; }
        #endregion

        #region rels
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}
