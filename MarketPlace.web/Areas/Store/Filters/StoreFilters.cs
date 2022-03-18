using MarketPlace.application.Services.Interfaces;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Store.Filters
{
    public class PanelFilter : IAsyncActionFilter
    {
        private readonly IStoreService _storeService;
        public PanelFilter(IStoreService storeService)
        {
            _storeService = storeService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (await _storeService.IsUserHaveStore(context.HttpContext.User.UserId())) await next();
            else  context.HttpContext.Response.Redirect("/?noPanel=true");
            
        }

    }
}
