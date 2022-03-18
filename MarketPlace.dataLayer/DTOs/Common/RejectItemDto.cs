using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Common
{
   public class RejectItemDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="لطفا شرح را وارد کنید")]
        [Display(Name ="شرح")]
        public string RejectMessage { get; set; }
    }
}
