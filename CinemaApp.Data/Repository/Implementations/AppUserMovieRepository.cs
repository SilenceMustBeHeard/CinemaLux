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

        public bool Exists(Guid userId, Guid movieId)
            => GetAllAttached()
            .Any(um => um.AppUserId == userId 
            && um.MovieId == movieId);

        public Task<bool> ExistsAsync(Guid userId, Guid movieId)
            => GetAllAttached().
            AnyAsync(um => um.AppUserId == userId 
            && um.MovieId == movieId);

        public AppUserMovie? GetByCompositeKey(Guid userId, Guid movieId)
            => GetAllAttached().
            SingleOrDefault(um => um.AppUserId == userId 
            && um.MovieId == movieId);

        public Task<AppUserMovie?> GetByCompositeKeyAsync(Guid userId, Guid movieId)
            => GetAllAttached()
            .SingleOrDefaultAsync(um => um.AppUserId == userId 
            && um.MovieId == movieId);
    }
}
