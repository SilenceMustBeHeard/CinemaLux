using CinemaApp.Web.ViewModels.Cinema;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<UsersCinemaIndexViewModel>> GetUserCinemasAsync();

        Task<CinemaProgramViewModel?> GetProgramAsync(string? cinemaId);

        Task<CinemaDetailsViewModel> GetCinemaDetailsAsync(string? cinemaId);
    }
}