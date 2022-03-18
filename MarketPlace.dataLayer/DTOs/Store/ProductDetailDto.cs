using MarketPlace.dataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Store
{
    public class ProductDetailDto
    {
        #region props
        public long ProductId { get; set; }
        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
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
        public List<ProductGallery> ProductGalleries{ get; set; }
        public List<ProductColor> ProductColors{ get; set; }
		public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
        public List<Product> RelatedProducts { get; set; }
        public dataLayer.Entities.Store.Store Store { get; set; }
		#endregion
	}
}
