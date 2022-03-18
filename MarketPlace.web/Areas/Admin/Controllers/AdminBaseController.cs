using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Route("Admin")]

    public class AdminBaseController : Controller
    {
            protected string ErrorMessage = "ErrorMessage";
            protected string SuccessMessage = "SuccessMessage";
            protected string InfoMessage = "InfoMessage";
            protected string WarningMessage = "WarningMessage";
    }
}
