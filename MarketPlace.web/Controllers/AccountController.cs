using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPlace.web.Controllers
{
    public class AccountController : SiteBaseController
    {
        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IEmailSender _emailSender;
        public AccountController(IEmailSender emailSender,IUserService userService,ICaptchaValidator captchaValidator)
        {
            _emailSender = emailSender;
            _captchaValidator = captchaValidator;
            _userService = userService;
        }
        #region Register
        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDto register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(register);
            }
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUser(register);
                switch (result)
                {
                    case RegisterUserResult.Success:
                        {
                            TempData[SuccessMessage] = "ثبت نام با موفقیت انجام شد";
                            TempData[InfoMessage] = "ایمیل  فعالسازی برای شما ارسال شد";
                            var userEmailActiveCode =await _userService.GetEmailActiveCodeByEmail(register.Email);
                            var emailMessage = Url.Action("ConfirmEmail","Account",new {email=register.Email,activeCode=userEmailActiveCode },Request.Scheme);
                           await _emailSender.SendEmailAsync(register.Email,"فعالسازی حساب کاربری",emailMessage);
                            return RedirectToAction("Login");
                        }
                         
                    case RegisterUserResult.EmailExist:
                        TempData[WarningMessage]= "ایمیل وارد شده تکراری می باشد";
                  ModelState.AddModelError("Email", "ایمیل وارد شده تکراری می باشد");
                        break;
                    case RegisterUserResult.Error:
                        break;
                    default:
                        break;
                }
               
            }
            return View(register);
        }
        #endregion
        #region Login
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Login(LoginUserDto login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(login);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(login);
                switch (result)
                {
                    case LoginUserResult.Success:
                        TempData[SuccessMessage] = "عملیات ورود موفقیت آمیز بود";
                        var user =await _userService.GetUserByEmail(login.Email);
                        var claims = new List<Claim> {
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.FirstName+" "+user.LastName)
                        };
                        var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties();
                        properties.IsPersistent = true;
                        await HttpContext.SignInAsync(principal,properties);
                        return Redirect("/");
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربری یافت نشد";
                        break;
                    case LoginUserResult.NotActivated:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نیست";
                        break;
                    default:
                        break;
                }
            }
            return View(login);
        }
        #endregion
        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
        #endregion

        #region forgot pass
        [HttpGet("forgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost("forgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(forgot);
            }
            if (ModelState.IsValid)
            {
                var result =await _userService.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه عبور جدید از طریق ایمیل برایتان ارسال شد";
                        TempData[InfoMessage] = " لطفا پس از ورود کلمه عبور خود را تغییر دهید";
                        return RedirectToAction("Login");
                    case ForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case ForgotPasswordResult.Error:
                        break;
                    default:
                        break;
                }
            }
            return View(forgot);
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string activeCode,string email)
        {
            var result = await _userService.ConfirmUserEmail(email,activeCode);
            if (result)
            {
                TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                return Redirect("/login");
            }
            else return NotFound();
        }
    }
}
