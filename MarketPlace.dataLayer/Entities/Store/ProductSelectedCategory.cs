using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Store
{
    public class ProductSelectedCategory:BaseEntity
    {
        #region props
        public long ProductId { get; set; }
        public long ProductCategoryId { get; set; }

        #endregion
        #region rels
        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }
        #endregion
    }
    }
