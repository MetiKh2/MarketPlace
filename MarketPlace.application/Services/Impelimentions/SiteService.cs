using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Site;
using MarketPlace.dataLayer.Entities.Site;
using MarketPlace.dataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Impelimentions
{
    public class SiteService : ISiteService
    {
        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;
        private readonly IGenericRepository<Slider> _sliderRepository;
        private readonly IGenericRepository<SiteBanner> _bannerRepository;
        public SiteService(IGenericRepository<SiteBanner> bannerRepository,IGenericRepository<SiteSetting> siteSettingRepository, IGenericRepository<Slider> sliderRepository)
        {
            _bannerRepository = bannerRepository;
            _sliderRepository = sliderRepository;
            _siteSettingRepository = siteSettingRepository;
        }
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if(_siteSettingRepository!=null)
            await _siteSettingRepository.DisposeAsync();

            if (_sliderRepository != null)
                await _sliderRepository.DisposeAsync();

            if (_bannerRepository != null)
                await _sliderRepository.DisposeAsync();
        }

       


        #endregion
        #region Site Setting

        public async Task<SiteSetting> GetDefaultSiteSetting()
        {
            return await _siteSettingRepository.GetQuery().FirstOrDefaultAsync(p => p.IsDefault && !p.IsRemoved);
        }

        public async Task<string> GetMobileSiteSettingDefault()
        {
            return await _siteSettingRepository.GetQuery().Where(p => p.IsDefault && !p.IsRemoved).Select(p => p.Mobile).FirstOrDefaultAsync();
        }
        #endregion
        #region Slider
        public async Task<List<Slider>> GetAllActiveSliders()
        {
            return await _sliderRepository.GetQuery().Where(p => p.IsActive).ToListAsync();
        }


        #endregion
        #region banner
        public async Task<List<SiteBanner>> GetSiteBannerByPlacement(List<BannerPlacement> placements)
        {
            return await _bannerRepository.GetQuery().Where(p => placements.Contains(p.BannerPlacement)).ToListAsync();
        }
        #endregion
    }
}
