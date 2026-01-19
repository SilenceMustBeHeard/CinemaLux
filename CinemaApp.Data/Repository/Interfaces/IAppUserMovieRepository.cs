using CinemaApp.Data.Models;
using System;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface IAppUserMovieRepository :
        IRepository<AppUserMovie, Guid>, IRepositoryAsync<AppUserMovie, Guid>
    {
        bool Exists(Guid userId, Guid movieId);
        Task<bool> ExistsAsync(Guid userId, Guid movieId);

        AppUserMovie? GetByCompositeKey(Guid userId, Guid movieId);
        Task<AppUserMovie?> GetByCompositeKeyAsync(Guid userId, Guid movieId);
    }
}


