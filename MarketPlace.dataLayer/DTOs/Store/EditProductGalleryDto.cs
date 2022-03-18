using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Store
{
    public class EditProductGalleryDto
    {
        public long  Id { get; set; }
        public string ImageName { get; set; }
        [Display(Name ="اولویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DisplayPeriority { get; set; }
        public long ProductId { get; set; }
    }
    public enum EditProductGalleryResult
    {
        Success,
        NotFound,
        NotForUser
    }
}
