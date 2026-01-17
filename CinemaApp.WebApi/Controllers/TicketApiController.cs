using CinemaApp.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketApiController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IProjectionService _projectionService;


        public TicketApiController(IProjectionService projectionService, ITicketService ticketService)
        {  
            _ticketService = ticketService;
            _projectionService = projectionService;
        }


        [HttpPost("PurchaseTickets")]
        public async Task<ActionResult<int>> BuyTicket(string cinemaId, string movieId, int quantity, string showtime, string userId)
        {

            var availableTickets = await _projectionService.GetAvailableTickets(cinemaId, movieId, showtime);
            if (availableTickets < quantity)
            {
                return BadRequest("Not enough tickets available.");
            }
      

            var result = await _ticketService.PurchaseTickets(cinemaId, movieId, quantity, showtime, userId);
            if (!result)
            {
                return StatusCode(500, "Error purchasing tickets.");
            }

            return Ok("Tickets purchased successfully.");
        }
    }
}
