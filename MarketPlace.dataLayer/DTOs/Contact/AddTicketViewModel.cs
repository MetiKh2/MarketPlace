using MarketPlace.dataLayer.DTOs.Site;
using MarketPlace.dataLayer.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Contact
{
   public class AddTicketViewModel:CaptchaViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }
        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }
        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }
        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Message { get; set; }
    }
    public enum AddTicketResult
    {
        Success,
        Error
    }
}
