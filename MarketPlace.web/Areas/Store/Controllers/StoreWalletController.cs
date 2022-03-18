using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.StoreWallet;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Store.Controllers
{
    public class StoreWalletController : StoreBaseController
    {
        private readonly IStoreWalletService _storeWalletService;
        private readonly IStoreService _storeService;

        public StoreWalletController(IStoreWalletService storeWalletService,IStoreService storeService)
        {
            _storeWalletService = storeWalletService;
            _storeService = storeService;
        }
        [HttpGet("wallet")]
        public async Task<IActionResult>Index(FilterStoreWalletDto filter)
        {
            var store =await _storeService.GetLastActiveStoreByUserId(User.UserId());
            if (store == null) return NotFound();
            filter.StoreId = store.Id;
            filter.TakeEntity = 15;
            return View(await _storeWalletService.FilterStoreWallet(filter));
        }
    }
}
