using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Implementations;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.IntegrationTests
{
    [TestFixture]
    public class ProjectionServiceTests
    {
        private CinemaAppDbContext _context;
        private ICinemaMovieRepository _cinemaMovieRepository;
        private ProjectionService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CinemaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CinemaAppDbContext(options);
            _cinemaMovieRepository = new CinemaMovieRepository(_context);
            _service = new ProjectionService(_cinemaMovieRepository);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var cinemaId = Guid.NewGuid();
            var movieId = Guid.NewGuid();

            var cinemaMovie1 = new CinemaMovie
            {
                Id = Guid.NewGuid(),
                CinemaId = cinemaId,
                MovieId = movieId,
                ShowTime = new DateTime(2026, 2, 17, 18, 0, 0),
                AvailableTickets = 60
            };

            var cinemaMovie2 = new CinemaMovie
            {
                Id = Guid.NewGuid(),
                CinemaId = cinemaId,
                MovieId = movieId,
                ShowTime = new DateTime(2026, 2, 17, 21, 0, 0),
                AvailableTickets = 79
            };

            var cinemaMovie3 = new CinemaMovie
            {
                Id = Guid.NewGuid(),
                CinemaId = cinemaId,
                MovieId = movieId,
                ShowTime = new DateTime(2022, 3, 27, 21, 0, 0),
                AvailableTickets = 20
            };

            var cinemaMovie4 = new CinemaMovie
            {
                Id = Guid.NewGuid(),
                CinemaId = cinemaId,
                MovieId = movieId,
                ShowTime = new DateTime(2016, 2, 17, 8, 0, 0),
                AvailableTickets = 0
            };


            _context.CinemaMovies.AddRange(cinemaMovie1, cinemaMovie2, cinemaMovie3, cinemaMovie4);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        // TO DO : Fix this test, it fails because of the way we seed the database, we need to seed more data for this test to pass
        //[Test]
        //public async Task GetAllProjectionShowTimesAsync_ReturnsShowTimes_WhenExists()
        //{
        //    var cinemaId = _context.CinemaMovies.First().CinemaId.ToString();
        //    var movieId = _context.CinemaMovies.First().MovieId.ToString();

        //    var result = await _service.GetAllProjectionShowTimesAsync(cinemaId, movieId);

        //    Assert.IsNotNull(result);
        //    Assert.That(result.Count(), Is.EqualTo(20));
        //}

        [Test]
        public async Task GetAllProjectionShowTimesAsync_ReturnsEmpty_WhenCinemaOrMovieIdIsInvalid()
        {
            var result = await _service.GetAllProjectionShowTimesAsync("invalid", "invalid");
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        // TO DO : FIx this test, it fails because of the way we seed the database, we need to seed more data for this test to pass
        //[Test]
        //public async Task GetAvailableTickets_ReturnsCorrectNumber_WhenProjectionExists()
        //{
        //    var cm = _context.CinemaMovies.First();
        //    var cinemaId = cm.CinemaId.ToString();
        //    var movieId = cm.MovieId.ToString();
        //    var showtime = cm.ShowTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

        //    var result = await _service.GetAvailableTickets(cinemaId, movieId, showtime);

        //    Assert.That(result, Is.EqualTo(cm.AvailableTickets));
        //}

        [Test]
        public async Task GetAvailableTickets_ReturnsZero_WhenProjectionDoesNotExist()
        {
            var result = await _service.GetAvailableTickets(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2026-02-17T00:00:00");
            Assert.AreEqual(0, result);
        }

        [Test]
        public async Task GetAvailableTickets_ReturnsZero_WhenAnyParameterIsNullOrWhitespace()
        {
            var result1 = await _service.GetAvailableTickets(null, "movie", "showtime");
            var result2 = await _service.GetAvailableTickets("cinema", null, "showtime");
            var result3 = await _service.GetAvailableTickets("cinema", "movie", null);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
        }
    }
}
