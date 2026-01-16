using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Implementations
{
    public class WatchlistRepository : Interfaces.IWatchlistRepository
    {
        private readonly CinemaAppDbContext _context;

        public WatchlistRepository(CinemaAppDbContext context)
        {
            _context = context;
        }


        public IQueryable<AppUserMovie> GetAllAttached()
            => _context.AppUserMovies
                .AsNoTracking()
                .Where(um => um.IsActive)
                .Include(um => um.Movie);

        public async Task<bool> ExistsAsync(string userId, Guid movieId)
            => await _context.AppUserMovies
                .AnyAsync(um =>
                    um.AppUserId == userId &&
                    um.MovieId == movieId &&
                    um.IsActive);

        public async Task<AppUserMovie?> GetByCompositeKeyAsync(string userId, Guid movieId)
          =>   await _context.AppUserMovies
                .FirstOrDefaultAsync(um =>
                    um.AppUserId == userId &&
                    um.MovieId == movieId);

        public async Task AddAsync(AppUserMovie entity)
            => await _context.AppUserMovies.AddAsync(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
