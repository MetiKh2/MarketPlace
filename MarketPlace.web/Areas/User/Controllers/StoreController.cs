using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.User.Controllers
{
    public class StoreController : UserBaseController
    {
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService, ICaptchaValidator captchaValidator)
        {
            _captchaValidator = captchaValidator;
            _storeService = storeService;
        }
        #region request store
        [HttpGet("request-stores")]
        public async Task<IActionResult>StoreRequests(FilterRequestStoreDto filter)
        {
            filter.TakeEntity = 2;
            filter.StoreAcceptanceState = FilterStoreState.All;
            filter.UserId = User.UserId();
            return View(await _storeService.FilterRequestStores(filter)) ;
        }

        [HttpGet("request-store-panel")]
        public IActionResult RequestStorePanel()
        {
            return View();
        }
        [HttpPost("request-store-panel")]
        public async Task<IActionResult>RequestStorePanel(AddRequestStoreDto request)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(request.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(request);
            }
            if (ModelState.IsValid)
            {
                switch (await _storeService.AddRequestStore(request, User.UserId()))
                {
                    case AddRequestStoreResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "تیم ما به درخواست شما را بررسی می کند";
                        break;
                    case AddRequestStoreResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "در حال حاضر یک درخواست در حال پردازش دارید";
                        break;
                    case AddRequestStoreResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما دسترسی لازم را ندارید";
                        break;
                }
                return RedirectToAction(nameof(StoreRequests),"Store",new {area="User" });
            }
            return View(request);
        }
        #endregion
        #region edit request
        [HttpGet("edit-request-store/{requestId}")]
        public async Task<IActionResult> EditStoreRequest(long requestId)
        {
            var storeRequest =await _storeService.GetRequestStoreForEdit(requestId,User.UserId());
            if (storeRequest == null) return NotFound();
            return View(storeRequest);
        }
        [HttpPost("edit-request-store/{requestId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStoreRequest(EditRequestStoreDto request)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(request.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(request);
            }
            if (ModelState.IsValid)
            { 
                switch (await _storeService.EditRequestStore(request,User.UserId()))
                {
                    case EditRequestStoreResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ویرایش شد";
                        TempData[InfoMessage] = "درخواست شما دوباره بررسی خواهد شد";
                        return RedirectToAction("StoreRequests","Store",new { area="User"});
                    case EditRequestStoreResult.NotFound:
                        TempData[WarningMessage] = "درخواست یافت نشد";
                        return RedirectToAction("StoreRequests", "Store", new { area = "User" });
                }
            }
            return View(request);
        }
        #endregion
    }
}
