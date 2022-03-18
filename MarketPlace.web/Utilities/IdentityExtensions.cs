using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MarketPlace.web.Utilities
{
    public static class IdentityExtensions
    {
        public static long UserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(p=>p.Type==ClaimTypes.NameIdentifier);
                if(data!=null)
                return Convert.ToInt64(data.Value);
            }
            return default(long);
        }
        public static long UserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;
            return user.UserId();
        }
    }
}
