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

        public WatchListController(IWatchListService watchListService, UserManager<AppUser> userManager)
       : base(userManager) 
        {
            _watchListService = watchListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<WatchListViewModel> watchList = await _watchListService.GetWatchListByUserIdAsync(User.Identity.Name);
            return View(watchList);
        }
    }
















}
