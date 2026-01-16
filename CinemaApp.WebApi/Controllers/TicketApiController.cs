using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketApiController : ControllerBase
    {



        [HttpGet("AvailableTickets")]

        public async Task<ActionResult<int>> GetAvailableTickets(string cinemaId, string movieId, string showtime)
        {
            return 0;
        }

        [HttpPost("PurchaseTickets")]
        public async Task<ActionResult<int>> BuyTicket(string cinemaId, string movieId, int quantity, string showtime)
        {
            return 0;
        }
    }
}
