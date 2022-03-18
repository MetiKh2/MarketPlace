using MarketPlace.dataLayer.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.DTOs.Contact
{
   public class TicketDetailDto
    {
        public Ticket Ticket { get; set; }
        public List<TicketMessage> TicketMessages { get; set; }
    }
}
