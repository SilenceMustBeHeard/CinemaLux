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
    }
}
