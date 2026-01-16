using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketApiController : ControllerBase
    {

        [HttpGet]

        public async Task<ActionResult<int>> GetAvailableTickets()
        {
            return 0;
        }
    }
}
