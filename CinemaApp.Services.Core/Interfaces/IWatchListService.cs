using CinemaApp.Web.ViewModels.WatchList;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IWatchListService
    {




        Task<IEnumerable<WatchListViewModel>> GetWatchListByUserIdAsync(Guid userId);


        Task<bool> IsMovieInWatchListAsync(Guid userId, string movieId);

        Task ToggleWatchListAsync(Guid userId, string movieId);



    }
}
