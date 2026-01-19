using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class WatchListController : BaseController
    {
        private readonly IWatchListService _watchListService;

        public WatchListController(
            IWatchListService watchListService,
            UserManager<AppUser> userManager)
            : base(userManager)
        {
            _watchListService = watchListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            IEnumerable<WatchListViewModel> watchList =
                await _watchListService.GetWatchListByUserIdAsync(userId);

            return View(watchList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(string movieId)
        {
            if (string.IsNullOrWhiteSpace(movieId))
            {
                return BadRequest();
            }

            Guid userId = GetUserId();

            await _watchListService.ToggleWatchListAsync(userId, movieId);

            return RedirectToAction(nameof(Index));
        }
    }
}
