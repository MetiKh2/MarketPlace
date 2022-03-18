using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Common;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.web.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #region filter
        [HttpGet("product-list")]
        public async Task<IActionResult>Index(FilterProductsDto filter)
        {
            filter.TakeEntity = 2;
            return View(await _productService.FilterProducts(filter));
        }
        #endregion
        #region accept product
        [HttpGet("accept-product")]
        public async Task<IActionResult> AcceptProductRequest(long productId)
        {
            if (await _productService.AcceptProduct(productId)) return JsonResponeResult.SendStatus(JsonReqponseStatusType.Success, "محصول با موفقیت پذیرفته شد", null,true);
            else return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error, "خطایی به وجود آمده", null,true);
        }
        #endregion    
        #region accept product
        [HttpPost("reject-product")]
        public async Task<IActionResult> RejectProductRequest(RejectItemDto reject)
        {
            if (ModelState.IsValid)
            {
                if (await _productService.RejectProduct(reject)) return JsonResponeResult.SendStatus(JsonReqponseStatusType.Success, "محصول با موفقیت رد شد", null,true);
                else return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error, "خطایی به وجود آمده", null,true);
            }
            return JsonResponeResult.SendStatus(JsonReqponseStatusType.Error, "مقادیر را کامل وارد کنید", null,false);
        }
#endregion
    }
}
