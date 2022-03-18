using MarketPlace.application.Extensions;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.application.Utils;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Store.Controllers
{
    public class ProductController : StoreBaseController
    {
        #region ctor
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;

        public ProductController(IProductService productService, IStoreService storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }
        #endregion
        [HttpGet("products")]
        public async Task<IActionResult> Index(FilterProductsDto filter)
        {
            var store = await _storeService.GetLastActiveStoreByUserId(User.UserId());
            if (store == null) return NotFound();
            filter.StoreId = store.Id;
            filter.FilterProductState = FilterProductState.All;
            return View(await _productService.FilterProducts(filter));
        }

        #region create
        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.AllCateories = await _productService.GetAllActiveProductCategories();
            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDto product, IFormFile productImage)
        {

            if (ModelState.IsValid)
            {
                if (productImage != null)
                {
                    product.ImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);

                    if (!productImage.AddImageToServer(product.ImageName, PathExtension.ProductImageServer, 300, 200, PathExtension.ProductThumbImageServer))
                    {
                        TempData[ErrorMessage] = "خطایی به وجود آمده است";
                        return View(product);
                    }
                    var store = await _storeService.GetLastActiveStoreByUserId(User.UserId());
                    switch (await _productService.CreateProduct(product, store.Id))
                    {
                        case CreateProductResult.Success:
                            TempData[SuccessMessage] = "محصول با موفقیت برایتان ثبت شد";
                            return RedirectToAction("Index");
                        case CreateProductResult.Error:
                            TempData[ErrorMessage] = "خطایی به وجود آمده است";
                            break;
                    }
                }
                TempData[WarningMessage] = "برای محصول خود یک تصویر انتخاب کنید";
            }
            ViewBag.AllCateories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion
        #region edit
        [HttpGet("edit-prodoct/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);
            if (product == null)
                return NotFound();
            ViewBag.AllCateories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        [HttpPost("edit-prodoct/{productId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDto product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null)
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);

                    if (!productImage.AddImageToServer(imageName, PathExtension.ProductImageServer, 300, 200, PathExtension.ProductThumbImageServer, product.ImageName))
                    {
                        TempData[ErrorMessage] = "خطایی به وجود آمده است";
                        return Redirect("/");
                    }
                    product.ImageName = imageName;
                }
                else product.ImageName = null;
                switch (await _productService.EditProduct(product,User.UserId()))
                {
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "محصول با موفقیت ویرایش شد";
                        return RedirectToAction("Index");
                    case EditProductResult.NotFound:
                        TempData[WarningMessage] = "محصول یافت نشد";
                        return RedirectToAction("Index");
                    case EditProductResult.NotForUser:
                        TempData[ErrorMessage] = "محصول یافت نشد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.AllCateories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion

        #region product gallery
        #region list
        [HttpGet("product-gallery/{productId}")]
        public async Task<IActionResult> GetProductGalleries(long productId)
        {
            if (!await _productService.IsExistProductByStoreOwnerId(productId, User.UserId()))
            {
                TempData[WarningMessage] = "محصول یافت نشد";
                return RedirectToAction("Index");
            }
            ViewBag.productId = productId;
            var seller=await _storeService.GetLastActiveStoreByUserId(User.UserId());
            return View(await _productService.GetAllProductGalleriesInStorePanel(productId,seller.Id));
        }
        #endregion
        #region create
        [HttpGet("create-product-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId)
        {
            if (!await _productService.IsExistProductByStoreOwnerId(productId, User.UserId()))
            {
                TempData[WarningMessage] = "محصول یافت نشد";
                return RedirectToAction("Index");
            }
            ViewBag.productId = productId;
            return View();
        }
        [HttpPost("create-product-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(CreateOrEditProductGalleryDto gallery,IFormFile Image)
        {
            if (!await _productService.IsExistProductByStoreOwnerId(gallery.ProductId, User.UserId()))
            {
                TempData[WarningMessage] = "محصول یافت نشد";
                return RedirectToAction("Index");
            }
            ViewBag.productId = gallery.ProductId;
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    gallery.ImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(Image.FileName);

                    if (!Image.AddImageToServer(gallery.ImageName, PathExtension.ProductGalleryImageServer, 100, 100, PathExtension.ProductGalleryThumbImageServer))
                    {
                        TempData[ErrorMessage] = "خطایی به وجود آمده است";
                        return View(gallery);
                    }
                    var seller = await _storeService.GetLastActiveStoreByUserId(User.UserId());
                    switch (await _productService.CreateProductGallery(gallery,seller.Id))
                    {
                        case CreateOrEditProductGalleryResult.Success:
                            TempData[SuccessMessage] = "گالری با موفقیت اضافه شد";
                            return RedirectToAction("GetProductGalleries",new { productId =gallery.ProductId});
                        case CreateOrEditProductGalleryResult.NotFound:
                            TempData[WarningMessage] = "محصول یافت نشد";
                            return RedirectToAction("Index");
                        case CreateOrEditProductGalleryResult.NotForUser:
                            break;
                        default:
                            break;
                    }
                }
                else TempData[WarningMessage] = "لطفا تصویر را وارد کنید";
            }
            return View(gallery);
        }
        #endregion
        #region edit
        [HttpGet("edit-gallery/{galleryId}")]
        public async Task<IActionResult> EditGallery(long galleryId)
        {
            var store = await _storeService.GetLastActiveStoreByUserId(User.UserId());
            var mainGallery = await _productService.GetProductGalleryForEdit(galleryId, store.Id);
            if (mainGallery == null) return NotFound();
            return View(mainGallery);
        }
        [HttpPost("edit-gallery/{galleryId}")]
        public async Task<IActionResult> EditGallery(long galleryId,CreateOrEditProductGalleryDto gallery,IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image!=null)
                {
                   var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(Image.FileName);

                    if (!Image.AddImageToServer(imageName, PathExtension.ProductGalleryImageServer, 100, 100, PathExtension.ProductGalleryThumbImageServer,gallery.ImageName))
                    {
                        TempData[ErrorMessage] = "خطایی به وجود آمده است";
                        return View(gallery);
                    }
                    gallery.ImageName = imageName;  
                }
               else gallery.ImageName = null;
                var seller = await _storeService.GetLastActiveStoreByUserId(User.UserId());
                switch (await _productService.EditGallery(gallery, galleryId, seller.Id))
                {
                    case CreateOrEditProductGalleryResult.Success:
                        TempData[SuccessMessage] = "اطلاعات ویرایش شد";
                        return RedirectToAction("GetProductGalleries",new { productId=gallery.ProductId});
                    case CreateOrEditProductGalleryResult.NotFound:
                        TempData[WarningMessage] = "گالری یافت نشد";
                        return RedirectToAction("Index");
                    case CreateOrEditProductGalleryResult.NotForUser:
                        TempData[WarningMessage] = "گالری یافت نشد";
                        return RedirectToAction("Index");

                    default:
                        break;
                }

            }
            return View(gallery);
        }
        #endregion
        #endregion
    }
}
