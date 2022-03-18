using MarketPlace.dataLayer.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Interfaces
{
   public interface IContactService:IAsyncDisposable
    {
        #region contactUs
        Task CreateContactUs(CreateContactUsDto dto, string userIp, long? userId);
        #endregion
        #region Ticket
        Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket,long userId);
        Task<FilterTicketDto> FilterTickets(FilterTicketDto filter);
        Task<TicketDetailDto> GetTicketForShow(long ticketId,long userId);
        Task<AddTicketMessageResult> AddTicketMessage(AddTicketMessageDto message,long userId);
        #endregion
    }
}
