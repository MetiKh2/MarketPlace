using MarketPlace.dataLayer.DTOs.StoreWallet;
using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
    public interface IStoreWalletService:IAsyncDisposable
    {
        Task<FilterStoreWalletDto> FilterStoreWallet(FilterStoreWalletDto filter);
        Task<bool> AddWallet(StoreWallet wallet);
    }
}
