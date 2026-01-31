using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.Controllers;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class MoviesController : BaseController
{
    private readonly IMovieService _movieService;

    

    public MoviesController(IMovieService movieService, UserManager<AppUser> userManager)
        : base(userManager) 
    {
        _movieService = movieService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return View(movies);
    }

  

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(string id)
    {
        Guid? userId = GetUserId();

        var model = await _movieService.GetMovieDetailsByIdAsync(id, userId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }


  
}
