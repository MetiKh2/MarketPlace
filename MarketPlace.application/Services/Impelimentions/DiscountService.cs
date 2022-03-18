using MarketPlace.application.Services.Interfaces;
using MarketPlace.application.Utils;
using MarketPlace.dataLayer.DTOs.Discount;
using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.Entities.Discount;
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
    public class DiscountService : IDiscountService
    {
        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public DiscountService(IGenericRepository<ProductDiscount> productDiscountRepository,IGenericRepository<Product> productRepository)
        {
            _productDiscountRepository = productDiscountRepository;
            _productRepository = productRepository;
        }

        

        public async ValueTask DisposeAsync()
        {
            await _productDiscountRepository.DisposeAsync();
        }

        #region product discount
        public async Task<FilterProductDiscountDto> FilterProductDiscount(FilterProductDiscountDto filter)
        {
            var query = _productDiscountRepository.GetQuery();
            #region filter
            if (filter.ProductId != null && filter.ProductId > 0) query = query.Where(p => p.ProductId == filter.ProductId.Value).AsQueryable();
            if (filter.StoreId != null && filter.StoreId > 0) query = query.Where(p => p.Product.SellerId == filter.StoreId.Value).AsQueryable();
            #endregion
            #region paging
            var pager = dataLayer.DTOs.Paging.Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allDiscounts = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetProductDiscount(allDiscounts).SetPaging(pager);
        }
        public async Task<CreateProductDiscountResult> CreateProductDiscount(CreateProductDiscountDto discount,long storeId)
        {
            var product = await _productRepository.GetEntityById(discount.ProductId.Value);
            if (product == null||product.SellerId!=storeId) return CreateProductDiscountResult.ProductIsNotAccess;
            await _productDiscountRepository.AddEntity(new ProductDiscount
            {
                DiscountNumberUsage = discount.DiscountNumberUsage,
                Percentage = discount.Percentage,
                ExpireDate = discount.ExpireDate.ToMiladiDateTime(),
                ProductId=discount.ProductId,
            }) ;

            await _productDiscountRepository.SaveChangesAsync();
            return CreateProductDiscountResult.Success;
        }
        #endregion

    }
}
