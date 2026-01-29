using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CinemaManagementController : BaseAdminController

    {

        private readonly ICinemaManagementService _cinemaManagementService;
        private readonly IUserService _userService;

        public CinemaManagementController(
                ICinemaManagementService cinemaManagementService,
                IUserService userService,
                UserManager<AppUser> userManager)
                : base(userManager)
        {
            _cinemaManagementService = cinemaManagementService;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<CinemaManagementIndexViewModel> cinemas =
              await _cinemaManagementService.GetAllCinemaManagementAsync();

            return View(cinemas);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {


            var model = new CinemaManagementCreateViewModel()
            {
                Managers = await _userService.GetManagerEmailsAsync(),
            };

            return View(model);
        }












        [HttpPost]
        public async Task<IActionResult> Create(CinemaManagementCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Managers = await _userService.GetManagerEmailsAsync();
                return View(model);
            }

            await _cinemaManagementService.CreateCinemaAsync(model);
            return RedirectToAction(nameof(Manage));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _cinemaManagementService.GetCinemaForEditAsync(id);
            model.Managers = await _userService.GetManagerEmailsAsync();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(CinemaManagementEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Managers = await _userService.GetManagerEmailsAsync();
                return View(model);
            }

            await _cinemaManagementService.UpdateCinemaAsync(model);
            return RedirectToAction(nameof(Manage));
        }









        [HttpPost]
        public async Task<IActionResult> ToggleDelete(string id)
        {

            bool success = await _cinemaManagementService.ToggleDeleteCinemaAsync(id);
         if(!success)
            {
                TempData["ErrorMessage"] = "An error occurred while trying to delete the cinema.";
               
            }
            else
            {
                TempData["SuccessMessage"] = "Cinema deletion status toggled successfully.";
            }
            return RedirectToAction(nameof(Manage));
        }
    }
}
