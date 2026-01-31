using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.UserManagment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IManagerRepository _managerRepository;
        public UserService(UserManager<AppUser> userManager, IManagerRepository managerRepository)
        {
            _userManager = userManager;
            _managerRepository = managerRepository;
        }

        public async Task<IEnumerable<UserManagmentIndexViewModel>>
              GetUserManagmentBoardDataAsync(Guid userId)
        {
            var users = await _userManager.Users
                .Where(u => u.Id != userId)
                .ToListAsync();

            var result = new List<UserManagmentIndexViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserManagmentIndexViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Roles = roles
                });
            }

            return result;
        }



        public async Task<IEnumerable<SelectListItem>> GetManagerEmailsAsync()
        {
            var emails = await _managerRepository.GetAllAttached()
                .Where(m => m.User.Email != null)
                .Select(m => m.User.Email!)
                .ToArrayAsync();

            return emails.Select(e => new SelectListItem
            {
                Text = e,
                Value = e
            });
        }


        public async Task<(bool Failed, string ErrorMessage)> ChangeUserRoleAsync(
            ChangeUserRoleViewModel model,
            Guid adminId)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                return (true, "User not found.");
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return (true, "Failed to remove existing roles.");
            }
            var addResult = await _userManager.AddToRoleAsync(user, model.NewRole);
            if (!addResult.Succeeded)
            {
                return (true, "Failed to assign new role.");
            }
            return (false, string.Empty);
        }
    }

}