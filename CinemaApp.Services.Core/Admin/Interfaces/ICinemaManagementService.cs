using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Interfaces
{
    public interface ICinemaManagementService
    {
        Task<IEnumerable<CinemaManagementIndexViewModel>> GetAllCinemaManagementAsync();





        Task CreateCinemaAsync (CinemaManagementCreateViewModel model);
    }
}
