using MarketPlace.dataLayer.Entities.Common;
using MarketPlace.dataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Order
{
    public class OrderDetail:BaseEntity
    {
        #region props
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long? ProductColorId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int ProductColorPrice { get; set; }
        #endregion
        #region rel
        public Order Order { get; set; }
        public Product Product { get; set; }
        public ProductColor ProductColor { get; set; }
        #endregion
    }
}
