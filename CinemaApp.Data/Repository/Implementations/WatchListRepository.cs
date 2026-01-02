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
    public class WatchListRepository 
        : BaseRepository<AppUserMovie, object>, IWatchListRepository
    {
        public WatchListRepository(CinemaAppDbContext context)
            : base(context)
        {
        }

        public bool Exists(string userId, string movieId)
              => GetAllAttached()
            .Any(um
            => um.AppUserId == userId
            && um.MovieId.ToString() == movieId);

        public AppUserMovie? GetByCompositeKey(string userId, string movieId)
        => GetAllAttached()
            .SingleOrDefault(um 
            => um.AppUserId == userId 
            && um.MovieId.ToString() == movieId);

        public Task<bool> ExistsAsync(string userId, string movieId)
          => GetAllAttached()
            .AnyAsync(um
            => um.AppUserId == userId
            && um.MovieId.ToString() == movieId);

      

        public  Task<AppUserMovie?> GetByCompositeKeyAsync(string userId, string movieId)
            =>  GetAllAttached()
            .SingleOrDefaultAsync(um
            => um.AppUserId == userId
            && um.MovieId.ToString() == movieId);
    }
}
