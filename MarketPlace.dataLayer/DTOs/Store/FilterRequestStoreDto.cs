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
  public  class FilterRequestStoreDto:BasePaging
    {
        #region props
        public long? UserId { get; set; }
        public List<Entities.Store.Store> Stores { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public FilterStoreState StoreAcceptanceState { get; set; }
        #endregion
        #region methods
        public FilterRequestStoreDto SetStore(List<Entities.Store.Store> stores)
        {
            this.Stores = stores;
            return this;
        }
        public FilterRequestStoreDto SetPaging(BasePaging paging)
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
    public enum FilterStoreState
    {
        [Display(Name = "  همه  ")]
        All,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejjected
    }
} 
