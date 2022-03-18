using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.DTOs.StoreWallet;
using MarketPlace.dataLayer.Entities.Wallet;
using MarketPlace.dataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Impelimentions
{
    public class StoreWalletService : IStoreWalletService
    {
        private readonly IGenericRepository<StoreWallet> _storeWalletRepository;

        public StoreWalletService(IGenericRepository<StoreWallet> storeWalletRepository)
        {
            _storeWalletRepository = storeWalletRepository;
        }

        public async Task<bool> AddWallet(StoreWallet wallet)
        {
          await _storeWalletRepository.AddEntity(wallet);
            await _storeWalletRepository.SaveChangesAsync(); 
            return true;
        }

        public async ValueTask DisposeAsync()
        {
           await _storeWalletRepository.DisposeAsync();
        }

        public async Task<FilterStoreWalletDto> FilterStoreWallet(FilterStoreWalletDto filter)
        {
            var query =  _storeWalletRepository.GetQuery();
            #region filter
            if(filter.StoreId!=null&&filter.StoreId>0)query=query.Where(p=>p.StoreId==filter.StoreId.Value).AsQueryable();
            if (filter.PriceFrom != null && filter.PriceFrom >= 0) query = query.Where(p => p.Amount >= filter.PriceFrom.Value).AsQueryable();
            if (filter.PriceTo != null && filter.PriceTo >= 0) query = query.Where(p => p.Amount <= filter.PriceTo.Value).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allWallets = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetWallets(allWallets).SetPaging(pager) ;
        }
    }
}
