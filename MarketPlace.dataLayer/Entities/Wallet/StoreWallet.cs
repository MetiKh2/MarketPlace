using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Entities.Wallet
{
    public class StoreWallet:BaseEntity
    {
        public long StoreId { get; set; }
        public int Amount { get; set; }
        public TransactionType TransactionType{ get; set; }
        public string Description { get; set; }
        
        public Store.Store Store { get; set; }
    }
    public enum TransactionType
    {
        [Display(Name ="واریز")]
        Deposit,
        [Display(Name ="خروج")]
        Withdrawal
    }
}
