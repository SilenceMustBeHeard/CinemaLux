using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Implementations
{
    public class WatchListService : IWatchListService
    {
        private readonly IWatchlistRepository _watchListRepository;

        public WatchListService(IWatchlistRepository watchListRepository)
        {
            _watchListRepository = watchListRepository;
        }

        // -------------------- GET WATCHLIST --------------------
        public async Task<IEnumerable<WatchListViewModel>> GetWatchListByUserIdAsync(string userId)
            => await _watchListRepository.GetAllAttached()
                .AsNoTracking()
                .Where(um => um.AppUserId == userId && um.IsActive && !um.Movie.IsDeleted)
                .Select(um => new WatchListViewModel
                {
                    MovieId = um.MovieId.ToString(),
                    Title = um.Movie.Title,
                    Genre = um.Movie.Genre,
                    ReleaseDate = um.Movie.ReleaseDate.ToString("yyyy-MM-dd"),
                    ImageUrl = um.Movie.ImageUrl,
                    TrailerUrl = um.Movie.TrailerUrl
                })
                .ToListAsync();

        // -------------------- TOGGLE WATCHLIST --------------------
        public async Task ToggleWatchListAsync(string userId, string movieId)
        {
            if (!Guid.TryParse(movieId, out var movieGuid))
                return;

            var entry = await _watchListRepository.GetByCompositeKeyAsync(userId, movieGuid);

            if (entry == null)
            {
                await _watchListRepository.AddAsync(new AppUserMovie
                {
                    AppUserId = userId,
                    MovieId = movieGuid,
                    IsActive = true
                });
            }
            else
            {
                entry.IsActive = !entry.IsActive;
            }

            await _watchListRepository.SaveChangesAsync();
        }

        // -------------------- CHECK IF EXISTS --------------------
        public async Task<bool> IsMovieInWatchListAsync(string userId, string movieId)
        {
            if (!Guid.TryParse(movieId, out var movieGuid))
                return false;

            return await _watchListRepository.ExistsAsync(userId, movieGuid);
        }
    }
}
