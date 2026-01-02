using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Implementations
{
    public class AppUserMovieRepository : BaseRepository<AppUserMovie, Guid>, IAppUserMovieRepository
    {
        public AppUserMovieRepository(CinemaAppDbContext context)
            : base(context) { }

        public bool Exists(string userId, Guid movieId)
            => GetAllAttached()
            .Any(um => um.AppUserId == userId 
            && um.MovieId == movieId);

        public Task<bool> ExistsAsync(string userId, Guid movieId)
            => GetAllAttached().
            AnyAsync(um => um.AppUserId == userId 
            && um.MovieId == movieId);

        public AppUserMovie? GetByCompositeKey(string userId, Guid movieId)
            => GetAllAttached().
            SingleOrDefault(um => um.AppUserId == userId 
            && um.MovieId == movieId);

        public Task<AppUserMovie?> GetByCompositeKeyAsync(string userId, Guid movieId)
            => GetAllAttached()
            .SingleOrDefaultAsync(um => um.AppUserId == userId 
            && um.MovieId == movieId);
    }
}
