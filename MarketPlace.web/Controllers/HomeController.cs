using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Extensions;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.application.Utils;
using MarketPlace.dataLayer.DTOs.Contact;
using MarketPlace.dataLayer.Entities.Site;
using MarketPlace.web.Models;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IProductService productService;
        private readonly ISiteService _siteService;
        private readonly IPaymentService _paymentService;
        public HomeController(IPaymentService paymentService,ISiteService siteService,IContactService contactService, ICaptchaValidator captchaValidator,IProductService productService)
        {
            _paymentService = paymentService;
            _siteService = siteService;
            _captchaValidator = captchaValidator;
            this.productService = productService;
            _contactService = contactService;
        }
        #region contactUs
        [HttpGet("contact-us")]
        public IActionResult ContactUS()
        {
            return View();
        }
        [HttpPost("contact-us")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUS(CreateContactUsDto dto)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(dto.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(dto);
            }
            if (ModelState.IsValid)
            {
                await _contactService.CreateContactUs(dto,HttpContext.UserIP(),User.UserId());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return Redirect("contact-us");
            }
            return View(dto);
        }
        #endregion
        #region index
        public async Task<IActionResult> Index(bool noPanel)
        {
            
            if (noPanel == true) { TempData[WarningMessage] = "شما پنل فروشندگی ندارید"; return Redirect("/"); };
            ViewBag.Banners =await _siteService.GetSiteBannerByPlacement(new List<BannerPlacement> {
              BannerPlacement.Home_1,
              BannerPlacement.Home_2,
              BannerPlacement.Home_3
            });
            ViewData["OffProducts"] = await productService.GetAllProductOffers(12);
            return View();
        }
        #endregion

        #region About us
        [HttpGet("about-us")]
        public async Task< IActionResult >AboutUS()
        {
            var siteSetting =await _siteService.GetDefaultSiteSetting();
            return View(siteSetting);
        }
        #endregion

        #region Upload ckEdito
        [HttpPost("Uploader/UploadImage")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncName, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر انتخاب کنید";
                var notImage = JsonConvert.DeserializeObject("{'uploaded':0, 'error': {'message': \" " + notImageMessage + " \"}}");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            upload.AddImageToServer(fileName, PathExtension.UploadImageServer, null, null);
            return Json(new
            {
                uploaded = true,
                url = $"{PathExtension.UploadImage}{fileName}"
            });
        }
        #endregion

    }
}
