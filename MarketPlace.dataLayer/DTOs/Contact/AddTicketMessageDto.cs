using MarketPlace.dataLayer.DTOs.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Contact
{
   public class AddTicketMessageDto
    {
        public long TicketId { get; set; }
        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Message { get; set; }
    }

    public enum AddTicketMessageResult
    {
        NotForUser,
        NotFound,
        Success,
    }
}
  