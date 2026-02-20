using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Implementations;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.IntegrationTests
{
    [TestFixture]
    public class WatchListServiceTests
    {
        private CinemaAppDbContext _context;
        private IWatchlistRepository _watchListRepository;
        private WatchListService _service;
        private Guid _userId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CinemaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CinemaAppDbContext(options);
            _watchListRepository = new WatchlistRepository(_context);
            _service = new WatchListService(_watchListRepository);
            _userId = Guid.NewGuid();

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var movie1 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Inception",
                Director = "Christopher Nolan",
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO.",
                Genre = "Sci-Fi",
                ReleaseDate = new DateTime(2010, 7, 16),
                ImageUrl = "/images/inception.jpg"
            };

            var movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Interstellar",
                Director = "Christopher Nolan",
                Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                Genre = "Sci-Fi",
                ReleaseDate = new DateTime(2014, 11, 7),
                ImageUrl = "/images/interstellar.jpg"
            };

            _context.Movies.AddRange(movie1, movie2);

            var watchListEntry = new AppUserMovie
            {
                AppUserId = _userId,
                MovieId = movie1.Id,
                Movie = movie1,
                IsActive = true
            };

            _context.AppUserMovies.Add(watchListEntry);

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        // -------------------- GetWatchListByUserIdAsync --------------------
        [Test]
        public async Task GetWatchListByUserIdAsync_ReturnsActiveMoviesOnly()
        {
            var result = await _service.GetWatchListByUserIdAsync(_userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Inception", result.First().Title);
        }

        [Test]
        public async Task GetWatchListByUserIdAsync_ReturnsEmpty_WhenUserHasNoActiveMovies()
        {
            var emptyUserId = Guid.NewGuid();
            var result = await _service.GetWatchListByUserIdAsync(emptyUserId);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        // -------------------- ToggleWatchListAsync --------------------
        [Test]
        public async Task ToggleWatchListAsync_AddsEntry_WhenNotExists()
        {
            var movie = _context.Movies.First(m => m.Title == "Interstellar");

            await _service.ToggleWatchListAsync(_userId, movie.Id.ToString());

            var entry = _context.AppUserMovies.FirstOrDefault(um => um.AppUserId == _userId && um.MovieId == movie.Id);
            Assert.IsNotNull(entry);
            Assert.IsTrue(entry.IsActive);
        }

        [Test]
        public async Task ToggleWatchListAsync_TogglesEntry_WhenExists()
        {
            var movie = _context.Movies.First(m => m.Title == "Inception");

            // Toggle to inactive
            await _service.ToggleWatchListAsync(_userId, movie.Id.ToString());
            var entry = _context.AppUserMovies.First(um => um.AppUserId == _userId && um.MovieId == movie.Id);
            Assert.IsFalse(entry.IsActive);

            // Toggle back to active
            await _service.ToggleWatchListAsync(_userId, movie.Id.ToString());
            Assert.IsTrue(entry.IsActive);
        }

        [Test]
        public async Task ToggleWatchListAsync_DoesNothing_WhenMovieIdInvalid()
        {
            var initialCount = _context.AppUserMovies.Count();
            await _service.ToggleWatchListAsync(_userId, "invalid-guid");
            Assert.AreEqual(initialCount, _context.AppUserMovies.Count());
        }

        // -------------------- IsMovieInWatchListAsync --------------------
        [Test]
        public async Task IsMovieInWatchListAsync_ReturnsTrue_WhenActive()
        {
            var movie = _context.Movies.First(m => m.Title == "Inception");
            var result = await _service.IsMovieInWatchListAsync(_userId, movie.Id.ToString());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsMovieInWatchListAsync_ReturnsFalse_WhenInactive()
        {
            var movie = _context.Movies.First(m => m.Title == "Inception");
            var entry = _context.AppUserMovies.First(um => um.AppUserId == _userId && um.MovieId == movie.Id);
            entry.IsActive = false;
            _context.SaveChanges();

            var result = await _service.IsMovieInWatchListAsync(_userId, movie.Id.ToString());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsMovieInWatchListAsync_ReturnsFalse_WhenMovieIdInvalid()
        {
            var result = await _service.IsMovieInWatchListAsync(_userId, "invalid-guid");
            Assert.IsFalse(result);
        }
    }
}
