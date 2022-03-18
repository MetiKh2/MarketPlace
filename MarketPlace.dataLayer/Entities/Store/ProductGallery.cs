using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class ProductGallery:BaseEntity
    {
        #region props
         [Display(Name = "تصویر")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }
        public long ProductId { get; set; }
        public int DisplayPeriority { get; set; }
        #endregion
        #region Rel
        public Product Product { get; set; }
        #endregion
    }
}
