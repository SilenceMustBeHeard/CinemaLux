using CinemaApp.Web.ViewModels.Admin.UserManagment;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Services.Core.Admin.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagmentIndexViewModel>> GetUserManagmentBoardDataAsync(Guid userId);

        Task<IEnumerable<SelectListItem>> GetManagerEmailsAsync();

        Task<(bool Failed, string ErrorMessage)> ChangeUserRoleAsync(
            ChangeUserRoleViewModel model,
            Guid adminId);
    }
}