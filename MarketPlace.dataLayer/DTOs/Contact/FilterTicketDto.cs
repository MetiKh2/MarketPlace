using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Contact
{
   public class FilterTicketDto:BasePaging
    {
        #region props
        public string Title { get; set; }
        public long? UserId { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public List<Ticket> Tickets{ get; set; }
        public FilterTicketOrder OrderBy { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        #endregion

        #region methods
        public FilterTicketDto SetTicket(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }
        public FilterTicketDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.EndPage = paging.EndPage;
            this.StartPage = paging.StartPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.TakeEntity = paging.TakeEntity;
            this.PageCount = paging.PageCount;
            return this;
        }
        #endregion
    }

    public enum FilterTicketState
    {
        All,Deleted,NotDeleted
    }

    public enum FilterTicketOrder
    {
        CreateDate_Asc,
        CreateDate_Des,
    }
}
