using MarketPlace.dataLayer.DTOs.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Discount
{
    public class CreateProductDiscountDto:CaptchaViewModel
    {
        public long? ProductId { get; set; }
        [Range(0,100, ErrorMessage ="درصد سایت باید بین 0 تا 100 باشد")]
        public int Percentage { get; set; }
        public string ExpireDate { get; set; }
        public short DiscountNumberUsage { get; set; }

    }
    public enum CreateProductDiscountResult
    {
        Success,
        ProductIsNotAccess,
        Error
    }
}
