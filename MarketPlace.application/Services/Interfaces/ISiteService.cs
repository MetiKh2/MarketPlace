using MarketPlace.dataLayer.DTOs.Site;
using MarketPlace.dataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
    public interface ISiteService:IAsyncDisposable
    {
        #region  Site Setting
        Task<SiteSetting> GetDefaultSiteSetting();
        Task<string> GetMobileSiteSettingDefault();
        #endregion
        #region Slider
        Task<List<Slider>> GetAllActiveSliders();
        #endregion
        #region banner
        Task<List<SiteBanner>> GetSiteBannerByPlacement(List<BannerPlacement> placements);
        #endregion
    }
}
