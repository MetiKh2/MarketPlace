using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Discount
{
    public class ProductDiscount:BaseEntity
    {
        #region props
        public long? ProductId { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpireDate { get; set; }
        public short DiscountNumberUsage { get; set; }

        #endregion
        #region rels
        public ICollection<ProductDiscountUse> ProductDiscountUses { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}
