using MarketPlace.dataLayer.DTOs.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
    public interface IDiscountService:IAsyncDisposable
    {
        #region product discount
        Task<FilterProductDiscountDto> FilterProductDiscount(FilterProductDiscountDto filter);
        Task<CreateProductDiscountResult> CreateProductDiscount(CreateProductDiscountDto discount,long storeId);
        #endregion
    }
}
