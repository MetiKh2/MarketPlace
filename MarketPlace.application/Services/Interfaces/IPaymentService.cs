using MarketPlace.DataLayer.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
    public interface IPaymentService
    {
        PaymentStatus CreatePaymentRequest(string merchantId,int amount,string description,string callbackUrl,ref string redirectUrl,string userEmail);
        PaymentStatus PaymentVerfication(string merchantId,int amount,string authority,ref long refId);
        string GetAuthorityCodeFromCallback(HttpContext context);
    }
}
