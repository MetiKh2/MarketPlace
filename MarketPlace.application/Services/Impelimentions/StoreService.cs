using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Common;
using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.DTOs.Store;
using MarketPlace.dataLayer.Entities.Account;
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
    public class StoreService : IStoreService
    {
        private readonly IGenericRepository<Store> _storeRepository;
        private readonly IGenericRepository<User> _userRepository;
        public StoreService(IGenericRepository<User> userRepository, IGenericRepository<Store> storeRepository)
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
        }
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if (_storeRepository != null) await _storeRepository.DisposeAsync();
        }



        #endregion
        #region Store
        public async Task<AddRequestStoreResult> AddRequestStore(AddRequestStoreDto request, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user.IsBlocked) return AddRequestStoreResult.HasNotPermission;
            if (await _storeRepository.GetQuery().AnyAsync(p => p.UserId == userId && p.StoreAcceptanceState == StoreAcceptanceState.UnderProgress))
                return AddRequestStoreResult.HasUnderProgressRequest;

            await _storeRepository.AddEntity(new Store
            {
                UserId = userId,
                Phone = request.Phone,
                Address = request.Address,
                StoreName = request.StoreName,
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress,
                AdminDescription = "-"
            }) ;
            await _storeRepository.SaveChangesAsync();
            return AddRequestStoreResult.Success;

        }

        public async Task<FilterRequestStoreDto> FilterRequestStores(FilterRequestStoreDto filter)
        {
            var query = _storeRepository.GetQuery().Include(p => p.User).AsQueryable() ;
            #region state
            switch (filter.StoreAcceptanceState)
            {
                case FilterStoreState.All:
                    break;
                case FilterStoreState.UnderProgress:
                    query = query.Where(p => p.StoreAcceptanceState == StoreAcceptanceState.UnderProgress).AsQueryable();
                    break;
                case FilterStoreState.Accepted:
                    query = query.Where(p => p.StoreAcceptanceState == StoreAcceptanceState.Accepted).AsQueryable();
                    break;
                case FilterStoreState.Rejjected:
                    query = query.Where(p => p.StoreAcceptanceState == StoreAcceptanceState.Rejjected).AsQueryable();
                    break;
                default:
                    break;
            }
            #endregion
            #region filter
            if(filter.UserId!=null&&filter.UserId!=0)query = query.Where(p => p.UserId == filter.UserId).AsQueryable();
            if(!string.IsNullOrEmpty(filter.StoreName))query = query.Where(p => p.StoreName.Contains(filter.StoreName)).AsQueryable();
            if(!string.IsNullOrEmpty(filter.Phone))query = query.Where(p => p.Phone.Contains(filter.Phone)).AsQueryable();
            if(!string.IsNullOrEmpty(filter.Address))query = query.Where(p => p.Address.Contains(filter.Address)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allTickets = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetStore(allTickets).SetPaging(pager);

        }
        public async Task<EditRequestStoreDto> GetRequestStoreForEdit(long id, long userId)
        {
            //return await _storeRepository.GetQuery().Select(p => new EditRequestStoreDto()
            //{
            //    Id = p.Id,
            //    Address = p.Address,
            //    Phone = p.Phone,
            //    StoreName = p.StoreName
            //}).FirstOrDefaultAsync(p=>p.Id==id);
            var store =await _storeRepository.GetEntityById(id);
            if (store == null||store.UserId!=userId) return null;
            return new EditRequestStoreDto { 
            Id=store.Id,
            Address=store.Address,
            Phone=store.Phone,
            StoreName=store.StoreName
            };
        }

        public async Task<EditRequestStoreResult> EditRequestStore(EditRequestStoreDto request,long userId)
        {
            var storeRequest = await _storeRepository.GetEntityById(request.Id);
            if (storeRequest == null || storeRequest.UserId != userId) return EditRequestStoreResult.NotFound;
            storeRequest.StoreName = request.StoreName;
            storeRequest.Phone = request.Phone;
            storeRequest.Address = request.Address;
            storeRequest.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;
            _storeRepository.EditEntity(storeRequest);
            await _storeRepository.SaveChangesAsync();
            return EditRequestStoreResult.Success;
        }

        public async Task<bool> AcceptStoreRequest(long requestId)
        {
            var store = await _storeRepository.GetEntityById(requestId);
            if (store != null)
            {
                store.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                store.AcceptanceDescription = "اطلاعات پنل فروشندگی شما تایید شده است";
                _storeRepository.EditEntity(store);
                await _storeRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RejectStoreRequest(RejectItemDto reject)
        {
            var store = await _storeRepository.GetEntityById(reject.Id);
            if (store != null)
            {
                store.StoreAcceptanceState = StoreAcceptanceState.Rejjected;
                store.AcceptanceDescription = reject.RejectMessage;
                _storeRepository.EditEntity(store);
                await _storeRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Store> GetLastActiveStoreByUserId(long userId)
        {
           return await _storeRepository.GetQuery().OrderByDescending(p => p.CreateDate).FirstOrDefaultAsync(p=>p.StoreAcceptanceState==StoreAcceptanceState.Accepted&&p.UserId==userId);
        }

        public async Task<bool> IsUserHaveStore(long userId) => await _storeRepository.GetQuery().Include(p=>p.User).AnyAsync(p => p.UserId == userId&&!p.User.IsBlocked&&p.StoreAcceptanceState==StoreAcceptanceState.Accepted);
        #endregion


    }
}
