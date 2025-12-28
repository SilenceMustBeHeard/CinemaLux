using CinemaApp.Data;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var watchList = await _context.AppUserMovies
                .Include(um => um.Movie)
                .AsNoTracking()
                .Where(um => um.AppUserId.ToLowerInvariant() == userId.ToLowerInvariant())
                .Select(um => new WatchListViewModel
                {
                    MovieId = um.MovieId.ToString(),
                    Title = um.Movie.Title,
                    Genre = um.Movie.Genre,
                    ReleaseDate = um.Movie.ReleaseDate.ToString("yyyy-MM-dd"),
                    ImageUrl = um.Movie.ImageUrl,
                    TrailerUrl = um.Movie.TrailerUrl
                })
                .ToArrayAsync();

            return watchList;
        }

    }
}
