using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Utils
{
   public static  class PathExtension
    {
        #region slider
        public static string SliderOrigin = "/SiteTemplate/img/slider/";
        #endregion
        #region banner
        public static string BannerOrigin = "/SiteTemplate/img/bg/";
        #endregion
        #region user avatar
        public static string UserAvatarOriginPath = "/Content/Images/UserAvatar/origin/";
        public static string UserAvatarOriginSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

        public static string UserAvatarThumbPath = "/Content/Images/UserAvatar/Thumb/";
        public static string UserAvatarThumbSever = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumb/");
        #endregion
        #region default images
        public static string DefaultAvatarOrigin = "/Content/Images/Default/avatar.jpg";
        #endregion
        #region UploadImageServer
        public static string UploadImage = "/Content/Images/ckEditor/";
        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/ckEditor/");

        #endregion  
        #region product
        public static string ProductImage = "/Content/Images/Product/origin/";
        public static string ProductImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/Product/origin/"); 
        
        public static string ProductThumbImage = "/Content/Images/Product/thumb/";
        public static string ProductThumbImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/Product/thumb/");
        #endregion
        #region product gallery
        public static string ProductGalleryImage = "/Content/Images/ProductGallery/origin/";
        public static string ProductGalleryImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/ProductGallery/origin/");

        public static string ProductGalleryThumbImage = "/Content/Images/ProductGallery/thumb/";
        public static string ProductGalleryThumbImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/ProductGallery/thumb/");
        #endregion
        #region domain addres
        public static string DomainAddress = "https://localhost:44386";
        #endregion
    }
}
