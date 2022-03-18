using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Store
{
    public class EditProductDto
    {
        public long Id { get; set; }
        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
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
        public List<CreateProductColorDto> ProductColors { get; set; }
        public string ImageName { get; set; }
        public List<long> ProductSelectedCategories { get; set; }
        public List<CreateProductFeatureDto> ProductFeature { get; set; }
    }
    public enum EditProductResult
    {
        Success,
        NotFound,
        NotForUser
    }
}
