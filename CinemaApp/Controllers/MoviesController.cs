using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.Controllers;
using CinemaApp.Web.ViewModels.Movie;
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
    [Authorize(Roles = "Admin")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(MovieFormModelCreate model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _movieService.AddMovieAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(string id)
    {
        var movie = await _movieService.GetMovieDetailsByIdAsync(id);
        if (movie == null) 
            return NotFound();
        return View(movie);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string? id)
    {
        var movie = await _movieService.GetMovieForEditByIdAsync(id);
        if (movie == null) 
            return NotFound();
        return View(movie);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(MovieFormModelEdit model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _movieService.EditMovieAsync(model);
        return RedirectToAction("Details", new { id = model.Id });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var movie = await _movieService.GetMovieForDeleteByIdAsync(id);
        if (movie == null) return NotFound();
        return View(movie);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(MovieFormModelDelete model)
    {
        await _movieService.SoftDeleteMovieAsync(model.Id);
        return RedirectToAction("Index");
    }
}
