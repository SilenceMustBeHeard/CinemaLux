using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext _context;

        public MovieService(CinemaAppDbContext context)
        {
            _context = context;
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

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        // -------------------- ALL --------------------
        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            return await _context.Movies
                .AsNoTracking()
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
        }

        // -------------------- DETAILS --------------------
        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid movieId))
            {
                return null;
            }

            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
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
                    TrailerUrl = m.TrailerUrl
                })
                .FirstOrDefaultAsync();
        }

        // -------------------- EDIT (GET) --------------------
        public async Task<MovieFormModelEdit?> GetMovieForEditByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid movieId))
            {
                return null;
            }

            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new MovieFormModelEdit
                {
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

        // -------------------- EDIT (POST) --------------------
        public async Task EditMovieAsync(MovieFormModelEdit model)
        {
            if (!Guid.TryParse(model.Id, out Guid movieId))
                throw new ArgumentException("Invalid ID", nameof(model.Id));

            var movie = await _context.Movies.FindAsync(movieId);
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

            await _context.SaveChangesAsync();
        }

        // -------------------- SOFT DELETE --------------------
        public async Task<bool> SoftDeleteMovieAsync(string? id)
        {
            if (!Guid.TryParse(id, out var movieId))
                return false;

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == movieId && !m.IsDeleted);

            if (movie == null)
                return false;

            movie.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------- HARD DELETE --------------------
        public async Task<bool> DeleteMovieAsync(string? id)
        {
            if (!Guid.TryParse(id, out var movieId))
                return false;

            var movie = await _context.Movies
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
                return false;

            // Hard delete само ако вече е soft-deleted
            if (!movie.IsDeleted)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------- GET FOR DELETE VIEW --------------------
        public async Task<MovieFormModelDelete?> GetMovieForDeleteByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid movieId))
                return null;

            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new MovieFormModelDelete
                {
                    Id = m.Id.ToString(),
                    Title = m.Title
                })
                .FirstOrDefaultAsync();
        }


    }
}

    