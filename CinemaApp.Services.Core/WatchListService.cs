using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    public class WatchListService: IWatchListService
    {
    private readonly CinemaAppDbContext _context;
        public WatchListService(CinemaAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WatchListViewModel>> GetWatchListByUserIdAsync(string userId)
        {
            return await _context.AppUserMovies
                .AsNoTracking()
                .Where(um =>
                    um.AppUserId == userId &&
                    um.IsActive)
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
        }


        public async Task ToggleWatchListAsync(string userId, string movieId)
        {
            if (!Guid.TryParse(movieId, out Guid movieGuid))
            {
                return;
            }

            var entry = await _context.AppUserMovies
                .FirstOrDefaultAsync(um =>
                    um.AppUserId == userId &&
                    um.MovieId == movieGuid);

            if (entry == null)
            {
                _context.AppUserMovies.Add(new AppUserMovie
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

            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsMovieInWatchListAsync(string userId, string movieId)
        {
            if (!Guid.TryParse(movieId, out Guid movieGuid))
            {
                return false;
            }
            return await _context.AppUserMovies
                .AnyAsync(um =>
                    um.AppUserId == userId &&
                    um.MovieId == movieGuid &&
                    um.IsActive);
        }

    }
}
