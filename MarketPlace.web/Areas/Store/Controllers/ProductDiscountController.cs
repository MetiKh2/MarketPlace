using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Discount;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Store.Controllers
{
    public class ProductDiscountController : StoreBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly IStoreService _storeService;
        private readonly ICaptchaValidator _captchaValidator;
        public ProductDiscountController(IDiscountService discountService, IStoreService storeService, ICaptchaValidator captchaValidator)
        {
            _captchaValidator = captchaValidator;
            _discountService = discountService;
            _storeService = storeService;
        }
        [HttpGet("discounts")]
        public async Task<IActionResult> Index(FilterProductDiscountDto filter)
        {
            if (filter.ProductId == null || filter.ProductId < 1) return NotFound();
            var store = await _storeService.GetLastActiveStoreByUserId(User.UserId());
            filter.StoreId = store.Id;
            return View(await _discountService.FilterProductDiscount(filter));
        }

        [HttpGet("create-discount/{productId}")]
        public async Task<IActionResult> CreateProductDiscount(long productId)
        {
            return View(new CreateProductDiscountDto { 
            ProductId = productId
            });
        }
        [HttpPost("create-discount/{productId}"),AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateProductDiscount(CreateProductDiscountDto discount)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(discount.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(discount);
            }
            if (ModelState.IsValid)
            {
                var store = await _storeService.GetLastActiveStoreByUserId(User.UserId());

                switch (await _discountService.CreateProductDiscount(discount,store.Id))
                {
                    case CreateProductDiscountResult.Success:
                        TempData[SuccessMessage] = "تخفیف با موفقیت افزوده شد";
                        return RedirectToAction("Index",new { ProductId=discount.ProductId});
                    case CreateProductDiscountResult.ProductIsNotAccess:
                        TempData[ErrorMessage] = "محصول یافت نشد";
                        return RedirectToAction("Index","Product");
                    case CreateProductDiscountResult.Error:
                        break;
                    default:
                        break;
                }
            }
            return View(discount);
        }
    }
}
