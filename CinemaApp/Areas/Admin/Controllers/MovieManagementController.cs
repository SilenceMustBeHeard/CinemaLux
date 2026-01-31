using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Admin.Controllers
{
    public class MovieManagementController : BaseAdminController
    {
        private readonly IMovieManagementService _movieManagementService;

        public MovieManagementController(
                IMovieManagementService movieManagementService,
                UserManager<AppUser> userManager)
                : base(userManager)
        {
            _movieManagementService = movieManagementService;
        }

        public async Task<IActionResult> Manage()
        {
            var model = await _movieManagementService.GetAllMovieManagementAsync();
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormModelCreate model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _movieManagementService.AddMovieAsync(model);
            return RedirectToAction(nameof(Index));
        }













        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            var movie = await _movieManagementService.GetMovieForEditByIdAsync(id);
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

            await _movieManagementService.EditMovieAsync(model);
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var movie = await _movieManagementService.GetMovieForDeleteByIdAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MovieFormModelDelete model)
        {
            await _movieManagementService.SoftDeleteMovieAsync(model.Id);
            return RedirectToAction("Index");
        }
    }
    
}