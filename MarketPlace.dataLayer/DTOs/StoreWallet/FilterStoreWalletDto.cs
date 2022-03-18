using MarketPlace.dataLayer.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.StoreWallet
{
    public class FilterStoreWalletDto:BasePaging
    {
        public long? StoreId { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo{ get; set; }
        public List<Entities.Wallet.StoreWallet> StoreWallets { get; set; }
        #region methods
        public FilterStoreWalletDto SetWallets(List<Entities.Wallet.StoreWallet> product)
        {
            this.StoreWallets = product;
            return this;
        }
        public FilterStoreWalletDto SetPaging(BasePaging paging)
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
