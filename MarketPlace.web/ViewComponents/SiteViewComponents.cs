using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.Entities.Order;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.ViewComponents
{
    #region Site Header

    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public SiteHeaderViewComponent(IUserService userService, ISiteService siteService, IProductService productService)
        {
            _userService = userService;
            _siteService = siteService;
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Mobile = await _siteService.GetMobileSiteSettingDefault();
            if (User.Identity.IsAuthenticated) ViewBag.FullName = await _userService.GetUserFullName(User.UserId());
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            return View("SiteHeader");
        }
    }
    #endregion
    #region Site Footer

    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _siteService.GetDefaultSiteSetting();
            return View("SiteFooter", model);
        }
    }
    #endregion
    #region Homa Sliders  

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _siteService.GetAllActiveSliders();
            return View("HomeSlider", model);
        }
    }
    #endregion

    #region user order
    public class UserOrderViewComponent : ViewComponent
    {
        private readonly IOrderService _orderServive;
        public UserOrderViewComponent(IOrderService orderService)
        {
            _orderServive = orderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
                return View("UserOrder", await _orderServive.GetUserLatestOrder(User.UserId()));
            return View("NULL");
        }
    }
    #endregion
}
