using CinemaApp.Web.ViewModels.WatchList;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IWatchListService
    {




        Task<IEnumerable<WatchListViewModel>> GetWatchListByUserIdAsync(string userId);

        Task ToggleWatchListAsync(string userId, string movieId);

        Task<bool> IsMovieInWatchListAsync(string userId, string movieId);
    }
}
