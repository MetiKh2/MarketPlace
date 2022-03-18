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
  public interface IStoreService:IAsyncDisposable
    {
        #region Store
        Task<AddRequestStoreResult> AddRequestStore(AddRequestStoreDto request,long userId);
        Task<FilterRequestStoreDto> FilterRequestStores(FilterRequestStoreDto filter);
        Task<EditRequestStoreDto> GetRequestStoreForEdit(long id,long userId);
        Task<EditRequestStoreResult> EditRequestStore(EditRequestStoreDto request,long userId);
        Task<bool> AcceptStoreRequest(long requestId);
        Task<bool> RejectStoreRequest(RejectItemDto reject);
        Task<Store> GetLastActiveStoreByUserId(long userId);
        Task<bool> IsUserHaveStore(long userId);
        #endregion
    }
}
