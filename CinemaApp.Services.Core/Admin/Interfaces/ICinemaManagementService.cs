using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Interfaces
{
    public interface ICinemaManagementService: ICinemaService
    {
        Task<IEnumerable<CinemaManagementIndexViewModel>> GetAllCinemaManagementAsync();
        Task CreateCinemaAsync (CinemaManagementCreateViewModel model);

        Task<CinemaManagementEditViewModel> GetCinemaForEditAsync(string cinemaId);
        Task UpdateCinemaAsync(CinemaManagementEditViewModel model);



        Task<bool> ToggleDeleteCinemaAsync(string cinemaId);


    }
}
