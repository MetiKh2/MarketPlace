using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Utilities
{
    public static class HttpExtensions
    {
        public static string UserIP(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString() ;
        }
    }
}
