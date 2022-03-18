using MarketPlace.dataLayer.DTOs.Order;
using MarketPlace.dataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
    public interface IOrderService:IAsyncDisposable
    {
        #region order
        Task<long> AddOrderForUser(long userId);
        Task<Order> GetUserLatestOrder(long userId);
        Task<int> GetTotalOrderPriceForPayment(long userId);
        Task PayOrderProductPriceToStore(long userId,string refId);
        Task<bool> CloseUserOpenOrderAfterPayment(long userId,string trackingCode);
        #endregion

        #region order detail
        Task AddProductToOpenOrder(AddProductToOrderDto order,long userId);
        Task<bool> RemoveOrderDetail(long detailId, long userId);
        Task<bool> ChangeOrderDetailCount(long detailId,long userId, int count);
        #endregion
    }
}
