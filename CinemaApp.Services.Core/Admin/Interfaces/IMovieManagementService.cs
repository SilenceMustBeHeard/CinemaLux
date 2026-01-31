using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Interfaces
{
    public interface IMovieManagementService: IMovieService
    {
        Task<IEnumerable<MovieManagementIndexViewModel>> GetAllMovieManagementAsync();
      












    }
}
