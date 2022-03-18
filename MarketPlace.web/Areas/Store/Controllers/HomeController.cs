using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Store.Controllers
{
    public class HomeController : StoreBaseController
    {
        public async Task<IActionResult>Index()
        {
             
            return View();
        }
    }
}
