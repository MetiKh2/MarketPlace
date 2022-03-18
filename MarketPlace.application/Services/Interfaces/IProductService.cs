using MarketPlace.dataLayer.DTOs.Common;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.dataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
    public interface IProductService:IAsyncDisposable
    {
        #region product
        Task<FilterProductsDto> FilterProducts(FilterProductsDto filter);
        Task<CreateProductResult> CreateProduct(CreateProductDto product,long storeId);
        Task<bool> AcceptProduct(long productId);
        Task<bool> RejectProduct(RejectItemDto reject);
        Task<EditProductDto> GetProductForEdit(long productId);
        Task<EditProductResult> EditProduct(EditProductDto product,long userId);
        Task<bool> IsExistProductByStoreOwnerId(long productId,long userId);
        Task<bool> IsExistProductByStoreId(long productId,long storeId);
        Task<ProductDetailDto> GetProductDetail(long productId);
        Task<List<Product>> GetAllProductOffers(short take);
        #endregion
        #region category
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId);
      Task<List<ProductCategory>> GetAllActiveProductCategories();
        #endregion
        #region product selected category
        Task CreateProductSelectedCategories(List<long> selectedCategories,long productId);
        Task DeleteSelectedCategories(long productId);
        #endregion 
        #region product color
        Task CreateProductColors(List<CreateProductColorDto> colors, long productId);
        Task DeleteProductColors(long productId);
        #endregion
        #region product gallery
        Task<List<ProductGallery>> GetAllProductGalleries(long productId);
        Task<List<ProductGallery>> GetAllProductGalleriesInStorePanel(long productId,long sellerId);
        Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDto gallery , long sellerId);
        Task<CreateOrEditProductGalleryDto> GetProductGalleryForEdit(long galleryId,long storeId);
        Task<CreateOrEditProductGalleryResult> EditGallery(CreateOrEditProductGalleryDto gallery,long galleryId,long storeId);
        #endregion
        #region product features
        Task CreateProductFeatures(List<CreateProductFeatureDto> features, long productId);
        Task DeleteProductFeatures(long productId);
        #endregion
    }
}
