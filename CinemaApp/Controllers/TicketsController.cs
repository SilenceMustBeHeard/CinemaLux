using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Ticket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class TicketsController : BaseController
    {
        private readonly ITicketService _ticketService;

        public TicketsController(UserManager<AppUser> userManager, ITicketService ticketService) 
            : base(userManager)
        {
            _ticketService = ticketService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();


            IEnumerable<TicketIndexViewModel> userTickets =
                await _ticketService.GetUserTicketsAsync(userId);


            return View(userTickets);
        }
    }
}








