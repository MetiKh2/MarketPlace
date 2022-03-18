using MarketPlace.application.Services.Interfaces;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.web.Areas.Store.Filters;

namespace MarketPlace.web.Areas.Store.Controllers
{
    [Authorize]
    [Area("Store")]
    [Route("Store")]
    [ServiceFilter(typeof(PanelFilter))]
    public class StoreBaseController : Controller
    {
        //private readonly IStoreService _storeService;
        //public StoreBaseController(IStoreService storeService)
        //{
        //    _storeService = storeService;
        //}
        protected string ErrorMessage = "ErrorMessage";
        protected string SuccessMessage = "SuccessMessage";
        protected string InfoMessage = "InfoMessage";
        protected string WarningMessage = "WarningMessage";

        //public async Task<bool> IsUserHaveStore() => await _storeService.IsUserHaveStore(User.UserId());
    }
}
