using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Store;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.web.Controllers
{
    public class ProductController : SiteBaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("product-list")]
        public async Task<IActionResult> FilterProducts(FilterProductsDto filter)
        {
            filter.FilterProductState = FilterProductState.Active;
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            filter.TakeEntity = 4;
            return View(await _productService.FilterProducts(filter));
        }
        [HttpGet("product-detail/{productId}")]
        public async Task<IActionResult> ProductDetail(long productId)
        { 
            var product = await _productService.GetProductDetail(productId);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
