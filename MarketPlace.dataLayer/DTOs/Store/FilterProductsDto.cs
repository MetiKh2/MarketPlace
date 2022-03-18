using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Store
{
    public class FilterProductsDto:Paging.BasePaging
    {
        public FilterProductsDto()
        {
          //  OrderProduct = OrderProduct.CreateDate_Des;
        }

        #region props
        public string Title { get; set; }
        public long? StoreId { get; set; }
        public List<Product> Products { get; set; }
        public FilterProductState FilterProductState { get; set; }
        public long SelectedProductCategories { get; set; }
        public int FilterMinPrice { get; set; } 
        public int FilterMaxPrice { get; set; } 
        public int SelectedMinPrice { get; set; }  
        public int SelectedMaxPrice { get; set; }
        public OrderProduct OrderProduct { get; set; }
        #endregion
        #region methods
        public FilterProductsDto SetProducts(List<Product> product)
        {
            this.Products = product;
            return this;
        }
        public FilterProductsDto SetPaging(BasePaging paging)
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
    public enum FilterProductState
    {
        [Display(Name ="در حال بررسی")]
        UnderProgress,
        [Display(Name ="پذیرفته شده")]
        Accepted,
        [Display(Name ="رد شده")]
        Rejected,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیرفعال")]
        NotActive,
        [Display(Name = "همه")]
        All
    }
    public enum OrderProduct
    {
        [Display(Name = "بیشترین قیمت")]
        MaxPrice,
        [Display(Name = "کمترین قیمت")]
        MinPrice,
        [Display(Name = "نام")]
        Name,
        [Display(Name = "جدید ترین")]
        CreateDate_Des,
        [Display(Name = "قدیمی ترین")]
        CreateDate_Asc
    }
}
