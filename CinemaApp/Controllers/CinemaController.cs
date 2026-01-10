using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Cinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class CinemaController : BaseController
    {
        private readonly ICinemaService _cinemaService;

        public CinemaController(
            ICinemaService cinemaService,
            UserManager<AppUser> userManager)
            : base(userManager)
        {
            _cinemaService = cinemaService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UsersCinemaIndexViewModel> allCinemaUserView =
                await _cinemaService.GetUserCinemasAsync();

            return View(allCinemaUserView);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Program(string? Id)
        {
            var program = await _cinemaService
                .GetProgramAsync(Id);

            if(program == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(program);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details (string id)
        {
            var program = await _cinemaService
               .GetCinemaDetailsAsync(id);

            if (program == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(program);
        }
    }
}
    