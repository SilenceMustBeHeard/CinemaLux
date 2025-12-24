using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
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
                Title = model.Title,
                Genre = model.Genre,
                ReleaseDate = model.ReleaseDate,
                Director = model.Director,
                Duration = model.Duration,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                TrailerUrl = model.TrailerUrl
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
        public async Task<MovieFormModelCreate?> GetMovieForEditByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid movieId))
            {
                return null;
            }

            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new MovieFormModelCreate
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
        public async Task EditMovieAsync(string id, MovieFormModelCreate model)
        {
            if (!Guid.TryParse(id, out Guid movieId))
            {
                throw new ArgumentException("Invalid movie ID format.", nameof(id));
            }

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
            {
                throw new InvalidOperationException("Movie not found.");
            }

            movie.Title = model.Title;
            movie.Genre = model.Genre;
            movie.ReleaseDate = model.ReleaseDate;
            movie.Director = model.Director;
            movie.Duration = model.Duration;
            movie.Description = model.Description;
            movie.ImageUrl = model.ImageUrl;
            movie.TrailerUrl = model.TrailerUrl;

            await _context.SaveChangesAsync();
        }

       
    }
}
