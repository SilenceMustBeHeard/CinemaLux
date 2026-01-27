using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.UserManagment;
using Microsoft.AspNetCore.Identity;
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
                    Email = user.Email,
                    Roles = roles
                });
            }

            return result;
        }



        public async Task<IEnumerable<string>> GetManagerEmailsAsync()
          => await _managerRepository.GetAllAttached()
              .Where(m => m.User.Email != null)
              .Select(m => m.User.Email)
              .ToArrayAsync();


    }
}
