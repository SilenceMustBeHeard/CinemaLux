using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
   

    public class ErrorController : Controller
    {
        [Route("Error/NotImplemented")]
        public IActionResult NotImplemented()
        {
            return View();
        }
    }

}
