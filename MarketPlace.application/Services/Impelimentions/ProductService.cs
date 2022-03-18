using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Common;
using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.dataLayer.Entities.Store;
using MarketPlace.dataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Impelimentions
{
    public class ProductService : IProductService
    {
        #region ctor
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductColor> _productColorRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;
        private readonly IGenericRepository<ProductGallery> _productGalleryRepository;
        private readonly IGenericRepository<ProductFeature> _productFeaturesRepository;
        public ProductService(IGenericRepository<ProductFeature> productFeaturesRepository, IGenericRepository<ProductColor> productColorRepository, IGenericRepository<ProductGallery> productGalleryRepository, IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository, IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository = null)
        {
            _productFeaturesRepository = productFeaturesRepository;
            _productGalleryRepository=productGalleryRepository;
            _productColorRepository = productColorRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSelectedCategoryRepository = productSelectedCategoryRepository;
        }



        #endregion
        #region dispose
        public async ValueTask DisposeAsync()
        {
            if (_productRepository != null)
                await _productRepository.DisposeAsync();
            if (_productCategoryRepository != null)
                await _productCategoryRepository.DisposeAsync();
            if (_productSelectedCategoryRepository != null)
                await _productSelectedCategoryRepository.DisposeAsync();
            if (_productGalleryRepository != null)
                await _productGalleryRepository.DisposeAsync();
            if (_productFeaturesRepository != null)
                await _productFeaturesRepository.DisposeAsync(); 
            if (_productColorRepository != null)
                await _productColorRepository.DisposeAsync();
        }


        #endregion
        #region product
        public async Task<List<Product>> GetAllProductOffers(short take)
        {
            return await _productRepository.GetQuery().Include(p=>p.ProductDiscounts).Where(p => p.ProductDiscounts.Any(d=>d.ExpireDate>DateTime.Now&&d.DiscountNumberUsage>0)).Take(take).ToListAsync();
        }
        public async Task<ProductDetailDto> GetProductDetail(long productId)
        {
            var product =await _productRepository.GetQuery().Include(p=>p.ProductFeatures).Include(p=>p.Store).ThenInclude(p=>p.User).Include(p=>p.ProductSelectedCategories).ThenInclude(p=>p.ProductCategory).Include(p=>p.ProductColors).Include(p=>p.ProductGalleries).FirstOrDefaultAsync(p => p.IsActive && p.ProductAcceptanceState == ProductAcceptanceState.Accepted && p.Id == productId);
            if (product == null) return null;
            var selectedCategories = product.ProductSelectedCategories.Select(p => p.ProductCategoryId).ToList();
            
            return new ProductDetailDto { 
            Description = product.Description,
            Price = product.Price,
            ShortDescription = product.ShortDescription,
            Title = product.Title,
            Store = product.Store,
            ProductGalleries = product.ProductGalleries.ToList(),
            ProductColors = product.ProductColors.ToList(),
            ProductCategories=product.ProductSelectedCategories.Select(p=>p.ProductCategory).ToList(),
            ProductFeatures=product.ProductFeatures.ToList(),
            RelatedProducts=await _productRepository.GetQuery().Include(p=>p.ProductSelectedCategories).Where(p=>p.ProductAcceptanceState==ProductAcceptanceState.Accepted&&p.IsActive&&p.ProductSelectedCategories.Any(f=>selectedCategories.Contains(f.ProductCategoryId))&&p.Id!=product.Id).ToListAsync(),
            ProductId=productId,
            };
        }

        public async Task<FilterProductsDto> FilterProducts(FilterProductsDto filter)
        {
            var query = _productRepository.GetQuery();
            
            #region state
            switch (filter.FilterProductState)
            {
                case FilterProductState.UnderProgress:
                    query = query.Where(p => p.ProductAcceptanceState == ProductAcceptanceState.UnderProgress).AsQueryable();
                    break;
                case FilterProductState.Accepted:
                    query = query.Where(p => p.ProductAcceptanceState == ProductAcceptanceState.Accepted).AsQueryable();
                    break;
                case FilterProductState.Rejected:
                    query = query.Where(p => p.ProductAcceptanceState == ProductAcceptanceState.Rejected).AsQueryable();
                    break;
                case FilterProductState.Active:
                    query = query.Where(p => p.IsActive && p.ProductAcceptanceState == ProductAcceptanceState.Accepted).AsQueryable();
                    break;
                case FilterProductState.NotActive:
                    query = query.Where(p => !p.IsActive && p.ProductAcceptanceState == ProductAcceptanceState.Accepted).AsQueryable();
                    break;
                case FilterProductState.All:
                    break;
            }

            switch (filter.OrderProduct)
            {
                case OrderProduct.MaxPrice:
                    query = query.OrderByDescending(p => p.Price).AsQueryable();
                    break;
                case OrderProduct.MinPrice:
                    query = query.OrderBy(p => p.Price).AsQueryable();
                    break;
                case OrderProduct.Name:
                    query = query.OrderBy(p => p.Title).AsQueryable();
                    break;
                case OrderProduct.CreateDate_Des:
                    query = query.OrderByDescending(p => p.CreateDate).AsQueryable();
                    break;
                case OrderProduct.CreateDate_Asc:
                    query = query.OrderBy(p => p.CreateDate).AsQueryable();
                    break;
                default:
                    break;
            }
            #endregion
            #region filter
            filter.FilterMaxPrice = await query.MaxAsync(p => p.Price);
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => EF.Functions.Like(p.Title, $"%{filter.Title}%")).AsQueryable();
            if (filter.StoreId != null && filter.StoreId > 0) query = query.Where(p => p.SellerId == filter.StoreId.Value).AsQueryable();
            if(filter.SelectedProductCategories!=null&& filter.SelectedProductCategories >0) query = query.Include(p=>p.ProductSelectedCategories).Where(p =>p.ProductSelectedCategories.Any(p=>p.ProductCategoryId==filter.SelectedProductCategories)).AsQueryable();
            if (filter.SelectedMaxPrice<=0)filter.SelectedMaxPrice = filter.FilterMaxPrice;
            if (filter.SelectedMinPrice!=null&&filter.SelectedMinPrice>0) query = query.Where(p => p.Price >=filter.SelectedMinPrice).AsQueryable();
            if(filter.SelectedMaxPrice!=null&&filter.SelectedMaxPrice > 0) query = query.Where(p => p.Price <=filter.SelectedMaxPrice).AsQueryable();
            filter.FilterMinPrice = 0;
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allTickets = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetProducts(allTickets).SetPaging(pager);
        }

        public async Task<bool> AcceptProduct(long productId)
        {
            var product = await _productRepository.GetEntityById(productId);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Accepted;
                product.ProductAcceptOrRejectDescription = "محصول شما پذیزفته شد";
                _productRepository.EditEntity(product);
                await _productRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RejectProduct(RejectItemDto reject)
        {
            var product = await _productRepository.GetEntityById(reject.Id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Rejected;
                product.ProductAcceptOrRejectDescription = reject.RejectMessage;
                _productRepository.EditEntity(product);
                await _productRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductDto product, long storeId)
        {
            try
            {
                //create product
                var newProduct = new Product
                {
                    Description = product.Description,
                    IsActive = product.IsActive,
                    Price = product.Price,
                    ShortDescription = product.ShortDescription,
                    Title = product.Title,
                    SellerId = storeId,
                    ImageName = product.ImageName,
                    ProductAcceptanceState = ProductAcceptanceState.UnderProgress
                };
                await _productRepository.AddEntity(newProduct);
                await _productRepository.SaveChangesAsync();
                //create product categories
                await CreateProductSelectedCategories(product.ProductSelectedCategories, newProduct.Id);
                //create product color
                await CreateProductColors(product.ProductColors, newProduct.Id);
                await CreateProductFeatures(product.ProductFeature,newProduct.Id);
                await _productRepository.SaveChangesAsync();
                return CreateProductResult.Success;
            }
            catch
            {
                return CreateProductResult.Error;
            }
        }

        public async Task<EditProductDto> GetProductForEdit(long productId)
        {
            var product = await _productRepository.GetQuery().Include(p=>p.ProductFeatures).Include(p => p.ProductColors).Include(p => p.ProductSelectedCategories).FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                return null;
            return new EditProductDto
            {
                Id = product.Id,
                Description = product.Description,
                ImageName = product.ImageName,
                IsActive = product.IsActive,
                Price = product.Price,
                ProductColors = product.ProductColors.Select(p => new CreateProductColorDto { ColorName = p.ColorName, Price = p.Price,ColorCode=p.ColorCode }).ToList(),
                ProductSelectedCategories = product.ProductSelectedCategories.Select(p => p.ProductCategoryId).ToList(),
                ShortDescription = product.ShortDescription,
                Title = product.Title,
                ProductFeature=product.ProductFeatures.Select(p=>new CreateProductFeatureDto { ProductId=p.ProductId,Title=p.Title,Value=p.Value}).ToList(), 
                
            };
        }
        public async Task<EditProductResult> EditProduct(EditProductDto product, long userId)
        {
            var mainProduct = await _productRepository.GetQuery().Include(p => p.Store).SingleOrDefaultAsync(p => p.Id == product.Id);
            if (mainProduct == null) return EditProductResult.NotFound;
            if (mainProduct.Store.UserId != userId) return EditProductResult.NotForUser;
            mainProduct.Title = product.Title;
            mainProduct.ShortDescription = product.ShortDescription;
            mainProduct.Description = product.Description;
            mainProduct.Price = product.Price;
            mainProduct.ProductAcceptanceState = ProductAcceptanceState.UnderProgress;
            if (!string.IsNullOrEmpty(product.ImageName)) mainProduct.ImageName = product.ImageName;
            mainProduct.IsActive = product.IsActive;
            _productRepository.EditEntity(mainProduct);
            await DeleteProductColors(product.Id);
            await DeleteSelectedCategories(product.Id);
            await DeleteProductFeatures(product.Id);
            await CreateProductSelectedCategories(product.ProductSelectedCategories, product.Id);
            await CreateProductColors(product.ProductColors, product.Id);
            await CreateProductFeatures(product.ProductFeature, product.Id);
            await _productRepository.SaveChangesAsync();
            return EditProductResult.Success;
        }
        public async Task<bool> IsExistProductByStoreId(long productId, long storeId)
        {
            return await _productRepository.GetQuery().Include(p => p.Store).AnyAsync(p => p.Id == productId && p.SellerId == storeId);
        }

        public async Task<bool> IsExistProductByStoreOwnerId(long productId, long userId)
        {
            return await _productRepository.GetQuery().Include(p => p.Store).AnyAsync(p=>p.Id==productId&&p.Store.UserId==userId);
        }
        #endregion
        #region category
        public async Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return await _productCategoryRepository.GetQuery().Where(p => p.IsActive && !p.IsRemoved).ToListAsync();
        }
        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId)
        {
            if (parentId == null || parentId == 0)
                return await _productCategoryRepository.GetQuery().Where(p => !p.IsRemoved && p.IsActive && p.ParentId == null).ToListAsync();
            return await _productCategoryRepository.GetQuery().Where(p => !p.IsRemoved && p.IsActive && p.ParentId == parentId.Value).ToListAsync();
        }




        #endregion
        #region product selected categories
        public async Task CreateProductSelectedCategories(List<long> selectedCategories, long productId)
        {
            if (selectedCategories != null && selectedCategories.Any())
            {
                foreach (var item in selectedCategories)
                {
                    await _productSelectedCategoryRepository.AddEntity(new ProductSelectedCategory
                    {
                        ProductCategoryId = item,
                        ProductId = productId,
                    });
                }
            }
        }
        public async Task DeleteSelectedCategories(long productId)
        {
            var productSelectedCategories = await _productSelectedCategoryRepository.GetQuery().Where(p => p.ProductId == productId).ToListAsync();
            foreach (var item in productSelectedCategories)
                _productSelectedCategoryRepository.DeleteEntity(item);
        }
        #endregion
        #region product color
        public async Task CreateProductColors(List<CreateProductColorDto> colors, long productId)
        {
            if (colors != null && colors.Any())
            {
                foreach (var item in colors)
                {
                    await _productColorRepository.AddEntity(new ProductColor
                    {
                        ProductId = productId,
                        Price = item.Price,
                        ColorName = item.ColorName,
                        ColorCode = item.ColorCode,
                    });
                }
            }
        }



        public async Task DeleteProductColors(long productId)
        {
            var productColors = await _productColorRepository.GetQuery().Where(p => p.ProductId == productId).ToListAsync();
            foreach (var item in productColors)
                _productColorRepository.DeleteEntity(item);
        }


        #endregion
        #region product gallery
        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            return await _productGalleryRepository.GetQuery().Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<List<ProductGallery>> GetAllProductGalleriesInStorePanel(long productId, long sellerId)
        {
            return await _productGalleryRepository.GetQuery().Include(p=>p.Product).ThenInclude(p=>p.Store).Where(p => p.ProductId == productId&&p.Product.SellerId==sellerId).ToListAsync();
        }

        public async Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDto gallery, long sellerId)
        {
            if (!await IsExistProductByStoreId(gallery.ProductId, sellerId)) return CreateOrEditProductGalleryResult.NotFound;
            await _productGalleryRepository.AddEntity(new ProductGallery
            {
                ProductId = gallery.ProductId,
                DisplayPeriority = gallery.DisplayPeriority,
                ImageName = gallery.ImageName,
            });
            await _productGalleryRepository.SaveChangesAsync();
            return CreateOrEditProductGalleryResult.Success;
        }

        public async Task<CreateOrEditProductGalleryDto> GetProductGalleryForEdit(long galleryId, long storeId)
        {
            var gallery=await _productGalleryRepository.GetQuery().Include(p => p.Product).SingleOrDefaultAsync(p=>p.Product.SellerId==storeId&&p.Id==galleryId);
            if (gallery == null) return null;
            return new CreateOrEditProductGalleryDto
            {
                DisplayPeriority= gallery.DisplayPeriority,
                ImageName= gallery.ImageName,
                ProductId=gallery.ProductId,
            };
        }

        public async Task<CreateOrEditProductGalleryResult> EditGallery(CreateOrEditProductGalleryDto gallery, long galleryId, long storeId)
        {
            var mainGallery = await _productGalleryRepository.GetQuery().Include(p=>p.Product).SingleOrDefaultAsync(p =>  p.Id == galleryId);
            if (mainGallery == null) return CreateOrEditProductGalleryResult.NotFound;
            if (mainGallery.Product.SellerId != storeId) return CreateOrEditProductGalleryResult.NotForUser;
          if(!string.IsNullOrEmpty(gallery.ImageName)) mainGallery.ImageName = gallery.ImageName;
            mainGallery.DisplayPeriority = gallery.DisplayPeriority;
            _productGalleryRepository.EditEntity(mainGallery);
            await _productGalleryRepository.SaveChangesAsync();
            return CreateOrEditProductGalleryResult.Success;
        }



        #endregion
        #region product features
        public async Task CreateProductFeatures(List<CreateProductFeatureDto> features,long productId)
        {
            if (features != null && features.Any())
            {
                foreach (var item in features)
                {
                    await _productFeaturesRepository.AddEntity(new ProductFeature
                    {
                        ProductId = productId,
                        Title = item.Title,
                        Value = item.Value,
                    });
                }
                await _productFeaturesRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteProductFeatures(long productId)
        {
         foreach (var item in await _productFeaturesRepository.GetQuery().Where(p=>p.ProductId==productId).ToListAsync())
                _productFeaturesRepository.DeleteEntity(item);
        }

     
        #endregion
    }
}
