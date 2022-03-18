using GoogleReCaptcha.V3.Interface;
using MarketPlace.application.Services.Interfaces;
using MarketPlace.dataLayer.DTOs.Contact;
using MarketPlace.web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region cons
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IContactService _contactService;
        public TicketController(IContactService contactService, ICaptchaValidator captchaValidator)
        {
            _captchaValidator = captchaValidator;
            _contactService = contactService;
        }
        #endregion
        #region List
        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketDto filter)
        {
            filter.UserId = User.UserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_Des;
            return View(await _contactService.FilterTickets(filter));
        }
        #endregion
        #region Add
        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket() 
        {
            return View();
        }
        [HttpPost("add-ticket"),ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketViewModel ticket)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(ticket.Captcha))
            {
                TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
                return View(ticket);
            }
            if (ModelState.IsValid)
            {
                switch (await _contactService.AddUserTicket(ticket, User.UserId()))
                {
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "پاسخ شما به زودی ثبت خواهد شد";
                        return RedirectToAction("Index","Ticket");
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                        break;
                    default:
                        break;
                }

            }
            return View(ticket);
        }
        #endregion
        #region Show ticket detail
        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetail(long ticketId)
        {
            var ticket = await _contactService.GetTicketForShow(ticketId, User.UserId());
            if (ticket == null) return NotFound();
            return View(ticket);
        }
        #endregion
        #region AddTicketMessage
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketMessage(AddTicketMessageDto dto)
        {
            if (string.IsNullOrEmpty(dto.Message)) TempData[ErrorMessage]="لطفا متن پیام را وارد کنید";
            //if (!await _captchaValidator.IsCaptchaPassedAsync(message.Captcha))
            //{
            //    TempData[ErrorMessage] = "کپچا را به درستی وارد کنید";
            //    return RedirectToAction("TicketDetail", "Ticket", new { area = "User", ticketId = message.TicketId });
            //}
            if (ModelState.IsValid)
            {
                switch (await _contactService.AddTicketMessage(dto, User.UserId()))
                {
                    case AddTicketMessageResult.NotForUser:
                        return NotFound();
                    case AddTicketMessageResult.NotFound:
                        TempData[WarningMessage] = "تیکتی یافت نشد";
                        return RedirectToAction("Index", "Ticket", new { area = "User" });
                    case AddTicketMessageResult.Success:
                        TempData[SuccessMessage] = "پیغام شما با موفقیت ثبت شد";
                        break;
                }
            }
                return RedirectToAction("TicketDetail", "Ticket", new { area = "User", ticketId = dto.TicketId });
        }
        #endregion
    }
}
