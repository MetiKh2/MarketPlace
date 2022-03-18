using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.Entities.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Discount
{
    public class FilterProductDiscountDto:BasePaging
    {
        #region props
        public long? ProductId { get; set; }
        public long? StoreId { get; set; }
        public List<ProductDiscount> ProductDiscounts { get; set; }
        #endregion
        #region methods
        public FilterProductDiscountDto SetProductDiscount(List<ProductDiscount> discounts)
        {
            this.ProductDiscounts = discounts;
            return this;
        }
        public FilterProductDiscountDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.EndPage = paging.EndPage;
            this.StartPage = paging.StartPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.TakeEntity = paging.TakeEntity;
            this.PageCount = paging.PageCount;
            return this;
        }
        #endregion
    }
}
