using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Common;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.web.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Admin.Controllers
{
    public class StoreController : AdminBaseController
    {
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        #region Store Requests
        [HttpGet("store-requests")]
        public async Task<IActionResult> StoreRequests(FilterRequestStoreDto filter)
        {
            filter.TakeEntity =2;
            return View(await _storeService.FilterRequestStores(filter));
        }
        #endregion

        #region accept request
        [HttpGet("accept-request")]
        public async Task<IActionResult> AcceptStoreRequest(long requestId)
        {
            if (await _storeService.AcceptStoreRequest(requestId))
                return JsonResponeResult.SendStatus(JsonReqponseStatusType.Success, "درخواست با موفقیت تایید شد", null,true);
            else
                return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error, "اطلاعاتی با این مشخصات یافت نشد", null, true);
        }
        #endregion 
        #region reject request
        [HttpPost("reject-request")]
        public async Task<IActionResult> RejectStoreRequest(RejectItemDto reject)
        {
            if (await _storeService.RejectStoreRequest(reject))
                return JsonResponeResult.SendStatus(JsonReqponseStatusType.Success, "درخواست با موفقیت رد شد", null, true);
            else
                return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error, "اطلاعاتی با این مشخصات یافت نشد", null, true);
        }
        #endregion 
    }
}
