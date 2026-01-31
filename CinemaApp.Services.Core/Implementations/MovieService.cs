using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // -------------------- ADD --------------------
        public async Task AddMovieAsync(MovieFormModelCreate model)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = model.Title.Trim(),
                Genre = model.Genre,
                ReleaseDate = model.ReleaseDate,
                Director = model.Director,
                Duration = model.Duration,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                TrailerUrl = model.TrailerUrl,
                IsDeleted = false
            };

            await _movieRepository.AddAsync(movie);
        }

        // -------------------- GET ALL --------------------
        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
            => await _movieRepository.GetAllAttached()
                .AsNoTracking()
                .Where(m => !m.IsDeleted)
                .Select(m => new AllMoviesIndexViewModel
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate.ToString("yyyy-MM-dd"),
                    Director = m.Director,
                    Duration = m.Duration.ToString(),
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    TrailerUrl = m.TrailerUrl
                })
                .ToListAsync();

        // -------------------- DETAILS --------------------
        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string movieId, Guid? userId)
        {
            if (!Guid.TryParse(movieId, out Guid guid))
                return null;

            return await _movieRepository.GetAllAttached()
                .AsNoTracking()
                .Where(m => m.Id == guid && !m.IsDeleted)
                .Select(m => new MovieDetailsViewModel
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    Duration = m.Duration,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    TrailerUrl = m.TrailerUrl,
                    IsInWatchList = false 
                })
                .FirstOrDefaultAsync();
        }


        // -------------------- EDIT --------------------
        public async Task<MovieFormModelEdit?> GetMovieForEditByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid movieId))
                return null;

            return await _movieRepository.GetAllAttached()
                .AsNoTracking()
                .Where(m => m.Id == movieId && !m.IsDeleted)
                .Select(m => new MovieFormModelEdit
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    Duration = m.Duration,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    TrailerUrl = m.TrailerUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task EditMovieAsync(MovieFormModelEdit model)
        {
            if (!Guid.TryParse(model.Id, out Guid movieId))
                throw new ArgumentException("Invalid ID", nameof(model.Id));

            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie == null || movie.IsDeleted)
                throw new InvalidOperationException("Movie not found or deleted");

            movie.Title = model.Title.Trim();
            movie.Genre = model.Genre.Trim();
            movie.ReleaseDate = model.ReleaseDate;
            movie.Director = model.Director.Trim();
            movie.Duration = model.Duration;
            movie.Description = model.Description.Trim();
            movie.ImageUrl = model.ImageUrl;
            movie.TrailerUrl = model.TrailerUrl;

            await _movieRepository.UpdateAsync(movie);
        }

        // -------------------- SOFT DELETE --------------------
        public async Task<bool> SoftDeleteMovieAsync(string? id)
        {
            if (!Guid.TryParse(id, out var movieId))
                return false;

            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie == null || movie.IsDeleted)
                return false;

            return await _movieRepository.DeleteAsync(movie); // soft delete
        }

        // -------------------- HARD DELETE --------------------
        public async Task<bool> DeleteMovieAsync(string? id)
        {
            if (!Guid.TryParse(id, out var movieId))
                return false;

            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie == null || !movie.IsDeleted) // hard delete only if already soft-deleted
                return false;

            return await _movieRepository.HardDeleteAsync(movie);
        }


        // -------------------- GET FOR DELETE VIEW --------------------
        public async Task<MovieFormModelDelete?> GetMovieForDeleteByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid movieId))
                return null;

            return await _movieRepository.GetAllAttached()
                .AsNoTracking()
                .Where(m => m.Id == movieId && !m.IsDeleted)
                .Select(m => new MovieFormModelDelete
                {
                    Id = m.Id.ToString(),
                    Title = m.Title
                })
                .FirstOrDefaultAsync();
        }

    }
}
