using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;

namespace CinemaApp.Services.Core.Admin.Interfaces
{
    public interface ICinemaManagementService : ICinemaService
    {
        Task<IEnumerable<CinemaManagementIndexViewModel>> GetAllCinemaManagementAsync();

        Task CreateCinemaAsync(CinemaManagementCreateViewModel model);

        Task<CinemaManagementEditViewModel> GetCinemaForEditAsync(string cinemaId);

        Task UpdateCinemaAsync(CinemaManagementEditViewModel model);

        Task<bool> ToggleDeleteCinemaAsync(string cinemaId);
    }
}