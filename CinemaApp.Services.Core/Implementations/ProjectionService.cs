using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
