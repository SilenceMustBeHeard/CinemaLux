using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    public class WatchListController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<WatchListViewModel> watchList 
                = new HashSet<WatchListViewModel>();
            return View(watchList);
        }
    }
















}
