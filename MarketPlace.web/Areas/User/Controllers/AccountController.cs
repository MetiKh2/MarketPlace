using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Account;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IUserService _userService;
        public AccountController(ICaptchaValidator captchaValidator,IUserService userService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #region change pass
        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost("change-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto change)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(change.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(change);
            }
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePassword(change, User.UserId());
                switch (result)
                {
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "رمز عبور با موفقیت تغییر کرد";
                        TempData[InfoMessage] = "جهت تکمیل فرایند مجددا وارد سایت شوید ";
                        await HttpContext.SignOutAsync();
                        return Redirect("/login");

                    case ChangePasswordResult.WrongCurrentPassword:
                        ModelState.AddModelError("CurrentPassword", "رمز عبور فعلی نادرست می باشد");
                        break;
                    case ChangePasswordResult.Error:
                        TempData[WarningMessage] = "خطایی به وجود آمده است";
                        return RedirectToAction("ChangePassword");
                }
            }
            return View(change);
        }
        #endregion

        #region edit prof

        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditUserProfile()
        {
            var userProfile =await _userService.GetUserProfileForEdit(User.UserId());
            if (userProfile == null) return NotFound();
            return View(userProfile);
        }
        
        [HttpPost("edit-profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(EditUserProfileDto profile,IFormFile avatarImage)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(profile.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(profile);
            }
            if (ModelState.IsValid)
            {
                switch (await _userService.EditUserProfile(profile,User.UserId(),avatarImage))
                {
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "اطلاعات با موفقیت ویرایش شدند";
                        return RedirectToAction(nameof(EditUserProfile));
                    case EditUserProfileResult.Error:
                        TempData[ErrorMessage] = "ویرایش اطلاعات با موفقیت انجام نشد";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[WarningMessage] = "کاربری یافت نشد";
                        break;
                }
            }
            return View(profile);
        }
        #endregion
    }
}
