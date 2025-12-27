using CinemaApp.Data;
using CinemaApp.Services.Core;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CinemaApp.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService; // use _ for private fields

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> movies = await _movieService.GetAllMoviesAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(MovieFormModelCreate model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);


            }



            try
            {
                await _movieService.AddMovieAsync(model);
                return RedirectToAction("Index", "Movies");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the movie: " + ex.Message);
                return View(model);
            }
        }
        public async Task<IActionResult> Details(string id)
        {
            var movie = await _movieService.GetMovieDetailsByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var movie = await _movieService.GetMovieForEditByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, MovieFormModelCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _movieService.EditMovieAsync(id, model);
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while editing the movie: " + ex.Message);
                return View(model);
            }
        }
    }
}
