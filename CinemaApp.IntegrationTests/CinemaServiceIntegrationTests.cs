using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Implementations;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Cinema;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Services.IntegrationTests
{
    [TestFixture]
    public class CinemaServiceIntegrationTests
    {
        private CinemaAppDbContext _context;
        private ICinemaRepository _cinemaRepository;
        private CinemaService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CinemaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CinemaAppDbContext(options);
            _cinemaRepository = new CinemaRepository(_context);
            _service = new CinemaService(_cinemaRepository);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var movie1 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Inception",
                Duration = 148,
                Director = "Nolan",
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO.",
                Genre = "Sci-Fi",
            };

            var movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Interstellar",
                Duration = 169,
                Director = "Nolan",
                Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                Genre = "Sci-Fi",
            };

            var cinema = new Cinema
            {
                Id = Guid.NewGuid(),
                Name = "Arena",
                Location = "Sofia",
                CinemaMovies = new List<CinemaMovie>
                {
                    new CinemaMovie { Movie = movie1 },
                    new CinemaMovie { Movie = movie2 }
                }
            };

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetUserCinemasAsync_ReturnsAllCinemas()
        {
            var result = await _service.GetUserCinemasAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Arena", result.First().Name);
        }

        [Test]
        public async Task GetCinemaDetailsAsync_ReturnsDetails_WhenCinemaExists()
        {
            var cinemaId = _context.Cinemas.First().Id;

            var result = await _service.GetCinemaDetailsAsync(cinemaId.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual("Arena", result.Name);
            Assert.AreEqual(2, result.Movies.Count());
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Inception"));
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Interstellar"));
        }

        [Test]
        public async Task GetCinemaDetailsAsync_ReturnsNull_WhenCinemaDoesNotExist()
        {
            var result = await _service.GetCinemaDetailsAsync(Guid.NewGuid().ToString());
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProgramAsync_ReturnsProgram_WhenCinemaExists()
        {
            var cinemaId = _context.Cinemas.First().Id;

            var result = await _service.GetProgramAsync(cinemaId.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual("Arena", result.CinemaName);
            Assert.AreEqual(2, result.Movies.Count());
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Inception"));
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Interstellar"));
        }

        [Test]
        public async Task GetProgramAsync_ReturnsNull_WhenCinemaDoesNotExist()
        {
            var result = await _service.GetProgramAsync(Guid.NewGuid().ToString());
            Assert.IsNull(result);
        }
    }
}
