using MarketPlace.dataLayer.DTOs.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Account
{
   public class ChangePasswordDto:CaptchaViewModel
    {
        [Display(Name = "رمزعبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CurrentPassword { get; set; }
        [Display(Name = "رمزعبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NewPassword { get; set; }
        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
       [Compare("NewPassword",ErrorMessage ="رمز ها با هم مغایرت دارند")]
        public string ConfirmNewPassword { get; set; }
    }
    public enum ChangePasswordResult
    {
        Success,
        WrongCurrentPassword,
        Error
    }
}
