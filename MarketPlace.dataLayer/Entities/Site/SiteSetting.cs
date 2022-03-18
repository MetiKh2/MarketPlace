using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Site
{
    public class SiteSetting : BaseEntity
    {
        #region props
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }
        [Display(Name = "فوتر")]
        public string FooterText { get; set; }
        [Display(Name = "تلفن")]
        public string Phone { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "کپی رایت")]
        public string CopyRight { get; set; }
        [Display(Name = "نقشه")]
        public string MapScript { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "درباره ما")]
        public string AboutUS { get; set; }
        [Display(Name ="پیش فرض")]
        public bool IsDefault { get; set; }
        #endregion
    }
}
