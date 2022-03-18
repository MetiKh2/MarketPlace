using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Contacts
{
    public class TicketMessage : BaseEntity
    {
        #region props
        public long TicketId { get; set; }
        public long SenderId { get; set; }
        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Message { get; set; }
        #endregion
        #region rels
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
        [ForeignKey("SenderId")]
        public User User { get; set; }
        #endregion
    }
}
