using CinemaApp.Web.ViewModels.Admin.UserManagment;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagmentIndexViewModel>> GetUserManagmentBoardDataAsync(Guid userId);


        Task<IEnumerable<SelectListItem>> GetManagerEmailsAsync();
    }
}
