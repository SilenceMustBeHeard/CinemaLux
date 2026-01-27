using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaApp.Web.Areas.Admin.Controllers
{
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
                TempData["ErrorMessage"] = "Invalid data submitted. Please correct the errors and try again.";
                return View(model);
            }

        
            await _cinemaManagementService.CreateCinemaAsync(model);

            return RedirectToAction(nameof(Manage));
        }






    }
}
