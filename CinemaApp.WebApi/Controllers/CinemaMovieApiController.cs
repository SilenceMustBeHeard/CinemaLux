using CinemaApp.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaMovieApiController : ControllerBase
    {
        private readonly IProjectionService _projectionService;

        public CinemaMovieApiController(IProjectionService projectionService)
        {
            _projectionService = projectionService;
        }




        [HttpGet("showtimes")]
        public async Task<ActionResult<IEnumerable<string>>> GetProjectionShowtimes(string? cinemaId, string? movieId)
        {
            var showtimes = await _projectionService.GetAllProjectionShowTimesAsync(cinemaId, movieId);
            if (showtimes == null
                    || !showtimes.Any())
            {
                return NotFound();
            }
            return Ok(showtimes);
        }


        [HttpGet("AvailableTickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetAvailableTickets(string cinemaId, string movieId, string showtime)
        {
            var availableTickets = await _projectionService.GetAvailableTickets(cinemaId, movieId, showtime);
            if (availableTickets == null)
            {
                return NotFound();
            }
            return Ok(availableTickets);
        }
    }
}
