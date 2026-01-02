using CinemaApp.Data.Models;
using System;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface IAppUserMovieRepository :
        IRepository<AppUserMovie, Guid>, IRepositoryAsync<AppUserMovie, Guid>
    {
        bool Exists(string userId, Guid movieId);
        Task<bool> ExistsAsync(string userId, Guid movieId);

        AppUserMovie? GetByCompositeKey(string userId, Guid movieId);
        Task<AppUserMovie?> GetByCompositeKeyAsync(string userId, Guid movieId);
    }
}


