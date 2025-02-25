﻿using MarketPlace.application.Services.Interfaces;
using MarketPlace.application.Utils;
using MarketPlace.dataLayer.DTOs.Order;
using MarketPlace.web.Http;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.User.Controllers
{
    public class OrderController : UserBaseController
    {
        private readonly IOrderService orderService;
        private readonly IUserService userService;
        private readonly IPaymentService paymentService;

        public OrderController(IOrderService orderService,IUserService userService,IPaymentService paymentService)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.paymentService = paymentService;
        }

        #region add product to open order
        [HttpPost("add-product-to-order")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderDto order)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await orderService.AddProductToOpenOrder(order, User.UserId());
                    return JsonResponeResult.SendStatus(JsonReqponseStatusType.Success, "محصول با موفقیت به سبد خرید اضافه شد ", null, false);
                }
                else return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error, "برای خرید باید وارد سایت شوید ", null, false);
            }
            return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error,"در ثبت اطلعات خطایی رخ داد",null,false);
        }
        #endregion

        #region open cart
        [HttpGet("open-order")]
        public async Task<IActionResult> UserOpenOrder()
        {
            return View(await orderService.GetUserLatestOrder(User.UserId()));
        }
        #endregion

        #region remove product from order
        [HttpGet("remove-order-item/{detailId}")]
        public async Task<IActionResult> RemoveProductFromOrder(long detailId)
        {
            if (await orderService.RemoveOrderDetail(detailId, User.UserId()))
            {
                TempData[SuccessMessage] = "محصول با موفقیت حذف شد";
                //return PartialView(await orderService.GetUserLatestOrder(User.UserId()));
               return JsonResponeResult.SendStatus(JsonReqponseStatusType.Success, "محصول با موفقیت پاک شد", null, true);

            }
            return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error,"محصول یافت نشد", PartialView(await orderService.GetUserLatestOrder(User.UserId())), true);
        }
        #endregion

        #region open order partial
        [HttpGet("change-detail-count/{detailId}/{count}")]
        public async Task<IActionResult> ChangeDetailCount(long detailId, int count)
        {
            await orderService.ChangeOrderDetailCount(detailId, User.UserId(), count);
            return PartialView(await orderService.GetUserLatestOrder(User.UserId()));
             
        }
        #endregion
        #region pay order
        [HttpGet("pay-order")]
        public async Task<IActionResult> PayUserOrder()
        {
            var openOrderAmount = await orderService.GetTotalOrderPriceForPayment(User.UserId());
            string callBackUrl = PathExtension.DomainAddress+Url.RouteUrl("ZarinpalPaymentResult");
            string redirectUrl ="";
            var status = paymentService.CreatePaymentRequest(null,openOrderAmount,"خرید از سایت ما",callBackUrl,ref redirectUrl,null);
            if (status == DataLayer.DTOs.Common.PaymentStatus.St100) return Redirect(redirectUrl);
            return RedirectToAction("UserOpenOrder");
        }
        #endregion

        #region callback zarin pal
        [HttpGet("payment-result",Name ="ZarinpalPaymentResult")]
        public async Task<IActionResult> CallbackZarinPal()
        {
            string authority = paymentService.GetAuthorityCodeFromCallback(HttpContext);

            if (authority=="")
            {
                TempData[ErrorMessage] = "عملیات پرداخت با شکست مواجه شد";
            }
            var openOrderAmount = await orderService.GetTotalOrderPriceForPayment(User.UserId());
            long refId = 0;
            var res = paymentService.PaymentVerfication(null, openOrderAmount,authority,ref refId);
            if (res==DataLayer.DTOs.Common.PaymentStatus.St100)
            {
                TempData[SuccessMessage] = "پرداخت موفقیت آمیز بود";
                TempData[InfoMessage] = "کد پیگیری شما"+refId;
                await orderService.PayOrderProductPriceToStore(User.UserId(),refId.ToString());
                return View();
            }
            else
            {
                TempData[ErrorMessage] = "پرداخت موفقیت آمیز نبود";
                TempData[InfoMessage] = "کد پیگیری شما" + refId;
                return View();
            }
        }
        #endregion
    }
}
