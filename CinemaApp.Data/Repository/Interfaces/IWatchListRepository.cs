using CinemaApp.Data.Models;
using System.Linq;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface IWatchlistRepository
    {
        IQueryable<AppUserMovie> GetAllAttached();

        Task<AppUserMovie?> GetByCompositeKeyAsync(string userId, Guid movieId);

        Task<bool> ExistsAsync(string userId, Guid movieId);

        Task AddAsync(AppUserMovie entity);

        Task SaveChangesAsync();
    }
}
