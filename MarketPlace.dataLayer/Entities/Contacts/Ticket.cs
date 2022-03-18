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
    public class Ticket : BaseEntity
    {
        #region props
        public long OwnerId { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }
        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }
        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }
        [Display(Name = "وضعیت")]
        public TicketState TicketState{ get; set; }
        public bool IsReadByOwner { get; set; }
        public bool IsReadByAdmin { get; set; }
        #endregion
        #region rels
        [ForeignKey("OwnerId")]
        public User User { get; set; }
        public ICollection<TicketMessage> TicketMessages { get; set; }
        #endregion

    }

    public enum TicketSection
    {
        [Display(Name ="پشتیبانی")]
        Support,
        [Display(Name = "فنی")] 
        Technical,
        [Display(Name = "آموزشی")]
        Academic,
    }
    public enum TicketPriority
    {
        [Display(Name = "کم")] 
        Low,
        [Display(Name = "متوسط")] 
        Medium,
        [Display(Name = "زیاد")]
        High
    }
    public enum TicketState
    {
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "پاسخ داده شده")]
        Answerd,
        [Display(Name = "بسته")]
        Closed,
    }
}
