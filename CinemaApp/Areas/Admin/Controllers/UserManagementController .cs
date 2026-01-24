using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.UserManagment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.UserManagment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService _userService;

        public UserManagementController(
            IUserService userService,
            UserManager<AppUser> userManager) : base(userManager)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _userService
                .GetUserManagmentBoardDataAsync(this.GetUserId());

            return View(allUsers);
        }
    }
}

