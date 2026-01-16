using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Implementations
{
    public class ProjectionService : IProjectionService
    {
        private readonly ICinemaMovieRepository _cinemaMovieRepository;
        public ProjectionService(ICinemaMovieRepository cinemaMovieRepository)
        {
            _cinemaMovieRepository = cinemaMovieRepository;
        }

        public async Task<IEnumerable<string>> GetAllProjectionShowTimesAsync(string? cinemaId, string? movieId)
        {
            IEnumerable<string> showtimes = new List<string>();
            if(!string.IsNullOrWhiteSpace(cinemaId) && !string.IsNullOrWhiteSpace(movieId))
            {
                showtimes = await _cinemaMovieRepository.GetAllAttached()
                    .Where(cm => cm.CinemaId.ToString().ToLower() == cinemaId.ToLower()
                                 && cm.MovieId.ToString().ToLower() == movieId.ToLower())
                    .Select(cm => cm.ShowTime.ToString()) // check if any format fix needed
                    .ToListAsync();
            }
            return showtimes;
        }

        public async Task<int> GetAvailableTickets(string cinemaId, string movieId, string showtime)
        {
            var availableTickets = 0;
            if(string.IsNullOrWhiteSpace(cinemaId) 
                || string.IsNullOrWhiteSpace(movieId) 
                || string.IsNullOrWhiteSpace(showtime))
            {
                return await Task.FromResult(availableTickets);
            }
            var projection = await _cinemaMovieRepository.GetAllAttached()
                .SingleOrDefaultAsync(cm => cm.CinemaId.ToString().ToLower() == cinemaId.ToLower()
                             && cm.MovieId.ToString().ToLower() == movieId.ToLower()
                             && cm.ShowTime.ToString() == showtime);

            if(projection != null)
            {

             availableTickets =projection.AvailableTickets;
            }
            return await Task.FromResult(availableTickets);



        }

        public Task<bool> PurchaseTickets(string cinemaId, string movieId, int quantity, string showtime)
        {
            throw new NotImplementedException();
        }
    }
}
