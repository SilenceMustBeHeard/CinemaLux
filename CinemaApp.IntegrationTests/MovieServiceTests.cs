using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Implementations;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.IntegrationTests
{
    [TestFixture]
    public class MovieServiceTests
    {
        private CinemaAppDbContext _context;
        private IMovieRepository _movieRepository;
        private MovieService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CinemaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CinemaAppDbContext(options);
            _movieRepository = new MovieRepository(_context);
            _service = new MovieService(_movieRepository);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var movie1 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Inception",
                Genre = "Sci-Fi",
                ReleaseDate = new DateTime(2010, 7, 16),
                Director = "Nolan",
                Duration = 148,
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                ImageUrl = "/images/inception.jpg",
                TrailerUrl = "https://youtu.be/YoHD9XEInc0",
                IsDeleted = false
            };

            var movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Interstellar",
                Genre = "Sci-Fi",
                ReleaseDate = new DateTime(2014, 11, 7),
                Director = "Nolan",
                Duration = 169,
                Description = "Explorers travel through a wormhole in space.",
                ImageUrl = "/images/interstellar.jpg",
                TrailerUrl = "https://youtu.be/zSWdZVtXT7E",
                IsDeleted = false
            };

            _context.Movies.AddRange(movie1, movie2);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        
        [Test]
        public async Task GetAllMoviesAsync_ReturnsAllNonDeletedMovies()
        {
            var result = await _service.GetAllMoviesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(m => m.Title == "Inception"));
            Assert.IsTrue(result.Any(m => m.Title == "Interstellar"));
        }

        
        [Test]
        public async Task GetMovieDetailsByIdAsync_ReturnsMovie_WhenExists()
        {
            var movieId = _context.Movies.First().Id;

            var result = await _service.GetMovieDetailsByIdAsync(movieId.ToString(), null);

            Assert.IsNotNull(result);
            Assert.AreEqual("Inception", result.Title);
            Assert.IsFalse(result.IsInWatchList);
        }

        [Test]
        public async Task GetMovieDetailsByIdAsync_ReturnsNull_WhenMovieDoesNotExist()
        {
            var result = await _service.GetMovieDetailsByIdAsync(Guid.NewGuid().ToString(), null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddMovieAsync_AddsMovieSuccessfully()
        {
            var model = new MovieFormModelCreate
            {
                Title = "Dunkirk",
                Genre = "War",
                ReleaseDate = new DateTime(2017, 7, 21),
                Director = "Nolan",
                Duration = 106,
                Description = "Allied soldiers are evacuated from Dunkirk.",
                ImageUrl = "/images/dunkirk.jpg",
                TrailerUrl = "https://youtu.be/F-eMt3SrfFU"
            };

            await _service.AddMovieAsync(model);

            var added = await _context.Movies.FirstOrDefaultAsync(m => m.Title == "Dunkirk");
            Assert.IsNotNull(added);
            Assert.AreEqual("Nolan", added.Director);
        }

  
        [Test]
        public async Task EditMovieAsync_UpdatesMovieSuccessfully()
        {
            var movie = _context.Movies.First();
            var model = new MovieFormModelEdit
            {
                Id = movie.Id.ToString(),
                Title = "Inception Edited",
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate,
                Director = movie.Director,
                Duration = movie.Duration,
                Description = movie.Description,
                ImageUrl = movie.ImageUrl,
                TrailerUrl = movie.TrailerUrl
            };

            await _service.EditMovieAsync(model);

            var updated = await _context.Movies.FindAsync(movie.Id);
            Assert.AreEqual("Inception Edited", updated.Title);
        }

        [Test]
        public async Task SoftDeleteMovieAsync_DeletesMovieSuccessfully()
        {
            var movieId = _context.Movies.First().Id;

            var result = await _service.SoftDeleteMovieAsync(movieId.ToString());
            var movie = await _context.Movies.FindAsync(movieId);

            Assert.IsTrue(result);
            Assert.IsTrue(movie.IsDeleted);
        }

        [Test]
        public async Task SoftDeleteMovieAsync_ReturnsFalse_WhenMovieDoesNotExist()
        {
            var result = await _service.SoftDeleteMovieAsync(Guid.NewGuid().ToString());
            Assert.IsFalse(result);
        }

        
        [Test]
        public async Task DeleteMovieAsync_DeletesMovieSuccessfully()
        {
            var movie = _context.Movies.First();
            movie.IsDeleted = true;
            await _context.SaveChangesAsync();

            var result = await _service.DeleteMovieAsync(movie.Id.ToString());
            var deleted = await _context.Movies.FindAsync(movie.Id);

            Assert.IsTrue(result);
            Assert.IsNull(deleted);
        }

        [Test]
        public async Task DeleteMovieAsync_ReturnsFalse_WhenMovieIsNotSoftDeleted()
        {
            var movieId = _context.Movies.First(m => !m.IsDeleted).Id;
            var result = await _service.DeleteMovieAsync(movieId.ToString());
            Assert.IsFalse(result);
        }
    }
}
