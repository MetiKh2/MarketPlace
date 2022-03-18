using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class ProductCategory:BaseEntity
    {
        #region props
        public long? ParentId { get; set; }
        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UrlName { get; set; } 
        public bool IsActive { get; set; }
        #endregion
        #region rels
        [ForeignKey("ParentId")]
        public ProductCategory Parent { get; set; }
        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        #endregion
    }
}
