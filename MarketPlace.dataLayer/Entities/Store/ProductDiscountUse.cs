using MarketPlace.dataLayer.Entities.Account;
using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class ProductDiscountUse:BaseEntity
    {
        public long ProductDiscountId { get; set; }
        public long UserId { get; set; }

        #region rels
        public User User { get; set; }
        public ProductDiscount ProductDiscount { get; set; }
        #endregion
    }
}
