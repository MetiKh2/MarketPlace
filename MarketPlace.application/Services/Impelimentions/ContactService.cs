using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Contact;
using MarketPlace.dataLayer.DTOs.Paging;
using MarketPlace.dataLayer.Entities.Contacts;
using MarketPlace.dataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.application.Services.Impelimentions
{
    public class ContactService : IContactService
    {
        private readonly IGenericRepository<ContactUS> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;
        public ContactService(IGenericRepository<ContactUS> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _ticketMessageRepository = ticketMessageRepository;
            _ticketRepository = ticketRepository;
            _contactUsRepository = contactUsRepository;
        }


        #region ContactUs
        public async Task CreateContactUs(CreateContactUsDto dto, string userIp, long? userId)
        {
            var newContact = new ContactUS()
            {
                UserId = (userId != null && userId != 0) ? userId : null,
                Subject = dto.Subject,
                Email = dto.Email,
                UserIP = userIp,
                Text = dto.Text,
                FullName = dto.FullName,
            };
            await _contactUsRepository.AddEntity(newContact);
            await _contactUsRepository.SaveChangesAsync();
        }


        #endregion
        #region ticket
        public async Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId)
        {
            if (string.IsNullOrEmpty(ticket.Message)) return AddTicketResult.Error;
            //add ticket
            var newTicket = new Ticket
            {
                OwnerId = userId,
                IsReadByOwner = true,
                TicketPriority = ticket.TicketPriority,
                Title = ticket.Title,
                TicketSection = ticket.TicketSection,
                TicketState = TicketState.UnderProgress,
            };
            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChangesAsync();
            //add ticket message
            await _ticketMessageRepository.AddEntity(new TicketMessage
            {
                Message = ticket.Message,
                TicketId = newTicket.Id,
                SenderId = userId,
            });
            await _ticketMessageRepository.SaveChangesAsync();
            return AddTicketResult.Success;
        }
        #endregion
        public async ValueTask DisposeAsync()
        {
            if (_contactUsRepository != null)
                await _contactUsRepository.DisposeAsync();
            if (_ticketMessageRepository != null)
                await _ticketMessageRepository.DisposeAsync();
            if (_ticketRepository != null)
                await _ticketRepository.DisposeAsync();
        }

        public async Task<FilterTicketDto> FilterTickets(FilterTicketDto filter)
        {
            var query = _ticketRepository.GetQuery();
            #region state
            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(p => p.IsRemoved).AsQueryable();
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(p => !p.IsRemoved).AsQueryable();
                    break;
            }
            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDate_Asc:
                    query = query.OrderBy(p => p.CreateDate).AsQueryable();
                    break;
                case FilterTicketOrder.CreateDate_Des:
                    query = query.OrderByDescending(p => p.CreateDate).AsQueryable();
                    break;
            }
            #endregion
            #region filter
            if (filter.TicketSection != null) query = query.Where(p => p.TicketSection == filter.TicketSection.Value).AsQueryable();
            if (filter.TicketPriority != null) query = query.Where(p => p.TicketPriority == filter.TicketPriority.Value).AsQueryable();
            if (filter.UserId != null && filter.UserId != 0) query = query.Where(p => p.OwnerId == filter.UserId.Value).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => EF.Functions.Like(p.Title, $"%{filter.Title}%")).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allTickets =await query.Paging(pager).ToListAsync() ;
            #endregion
            return filter.SetTicket(allTickets).SetPaging(pager) ;
        }

        public async Task<TicketDetailDto> GetTicketForShow(long ticketId, long userId)
        {
            if (ticketId == null || userId == null) return null;
            var ticket = await _ticketRepository.GetQuery().Include(p=>p.TicketMessages).Include(p=>p.User).FirstOrDefaultAsync(p=>p.Id==ticketId);
            if (ticket == null || ticket.OwnerId != userId) return null;
            return new TicketDetailDto { 
            Ticket=ticket,
            TicketMessages=ticket.TicketMessages.Where(p=>!p.IsRemoved).OrderByDescending(p=>p.CreateDate).ToList()
            };
        }

        public async Task<AddTicketMessageResult> AddTicketMessage(AddTicketMessageDto message,long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(message.TicketId);
            if (ticket == null) return AddTicketMessageResult.NotFound;
            if (ticket.OwnerId != userId) return AddTicketMessageResult.NotForUser;
            ticket.IsReadByOwner = true;
            ticket.IsReadByAdmin = false;
            await _ticketMessageRepository.AddEntity(new TicketMessage { 
            Message=message.Message,
            TicketId=message.TicketId,
            SenderId=userId,
            });
           await _ticketMessageRepository.SaveChangesAsync();
            return AddTicketMessageResult.Success;
        }
    }
}
