using MarketPlace.dataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.EntitiesExtensions
{
    public static class OrderExtensions
    {
        public static string OrderDetailWithDiscountPrice(this OrderDetail orderDetail)
        {
            var discountPercentage = (100 - (orderDetail.Product.ProductDiscounts.OrderByDescending(a => a.CreateDate).FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage));
            double okDiscount = 1;
            if (discountPercentage != null && discountPercentage > 0)
                okDiscount = (double)discountPercentage / 100;
            else return "----";
            return ((orderDetail.Product.Price + orderDetail.ProductColor?.Price) * okDiscount)?.ToString("n0") ;
        }
        public static string OrderDetailWithDiscountPriceAndCount(this OrderDetail orderDetail)
        {
            var discountPercentage = (100 - (orderDetail.Product.ProductDiscounts.OrderByDescending(a => a.CreateDate).FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage));
            double okDiscount = 1;
            if (discountPercentage != null && discountPercentage > 0)
                okDiscount = (double)discountPercentage / 100;
            return ((orderDetail.Product.Price + orderDetail.ProductColor?.Price)*orderDetail.Count * okDiscount)?.ToString("n0");
        }
        public static int IntOrderDetailWithDiscountPriceAndCount(this OrderDetail orderDetail)
        {
            var discountPercentage = (100 - (orderDetail.Product.ProductDiscounts.OrderByDescending(a => a.CreateDate).FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage));
            double okDiscount = 1;
            if (discountPercentage != null && discountPercentage > 0)
                okDiscount = (double)discountPercentage / 100;
            return (int)((orderDetail.Product.Price + orderDetail.ProductColor.Price) * orderDetail.Count * okDiscount);
        }
        public static string StringSumOrderDetailsWithDiscount(this List<OrderDetail> orderDetails)
        {
            int total = 0;
            foreach (var item in orderDetails)
            {
                var discountPercentage = (100 - (item.Product.ProductDiscounts.OrderByDescending(a => a.CreateDate).FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage));
                double okDiscount = 1;
                if (discountPercentage != null && discountPercentage > 0)
                    okDiscount = (double)discountPercentage / 100;
                total+=(int)((item.Product.Price + item.ProductColor?.Price) * item.Count * okDiscount);
            }
            return total.ToString("n0");
        }
        public static int IntSumOrderDetailsWithDiscount(this List<OrderDetail> orderDetails)
        {
            int total = 0;
            foreach (var item in orderDetails)
            {
                var discountPercentage = (100 - (item.Product.ProductDiscounts.OrderByDescending(a => a.CreateDate).FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage));
                double okDiscount = 1;
                if (discountPercentage != null && discountPercentage > 0)
                    okDiscount = (double)discountPercentage / 100;
                total += (int)((item.Product.Price + item.ProductColor?.Price) * item.Count * okDiscount);
            }
            return total;
        }
    }
}