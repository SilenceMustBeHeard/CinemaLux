using CinemaApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class ManagerController : BaseController
    {
        public ManagerController(UserManager<AppUser> userManager)
            : base(userManager)
        {
        }
        public IActionResult Index()
        {
            return Ok("You are logged as manager!");
        }
    }
}
