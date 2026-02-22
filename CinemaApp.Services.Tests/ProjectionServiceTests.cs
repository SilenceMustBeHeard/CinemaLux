using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.ServiceTests
{
    [TestFixture]
    public class ProjectionServiceTests
    {
        private Mock<ICinemaMovieRepository> _cinemaMovieRepoMock;
        private ProjectionService _projectionService;

        private Guid _cinemaId;
        private Guid _movieId;
        private List<CinemaMovie> _projections;

        [SetUp]
        public void SetUp()
        {
            _cinemaMovieRepoMock = new Mock<ICinemaMovieRepository>(MockBehavior.Strict);
            _projectionService = new ProjectionService(_cinemaMovieRepoMock.Object);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _cinemaId = Guid.NewGuid();
            _movieId = Guid.NewGuid();

            _projections = new List<CinemaMovie>
            {
                new CinemaMovie
                {
                    Id = Guid.NewGuid(),
                    CinemaId = _cinemaId,
                    MovieId = _movieId,
                    ShowTime = new DateTime(2026, 2, 17, 18, 0, 0),
                    AvailableTickets = 50
                },
                new CinemaMovie
                {
                    Id = Guid.NewGuid(),
                    CinemaId = _cinemaId,
                    MovieId = _movieId,
                    ShowTime = new DateTime(2026, 2, 17, 21, 0, 0),
                    AvailableTickets = 75
                },
                new CinemaMovie
                {
                    Id = Guid.NewGuid(),
                    CinemaId = Guid.NewGuid(), // different cinema
                    MovieId = _movieId,
                    ShowTime = new DateTime(2026, 2, 17, 20, 0, 0),
                    AvailableTickets = 100
                }
            };
        }

        #region GetAllProjectionShowTimesAsync


        //// TO DO : fix the test to check the format of the showtime string if needed, currently it just checks if the string contains the time part
        //[Test]
        //public async Task GetAllProjectionShowTimesAsync_Should_ReturnShowtimes_WhenExists()
        //{
        //    var mockQueryable = _projections.BuildMock();
        //    _cinemaMovieRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

        //    var result = await _projectionService.GetAllProjectionShowTimesAsync(_cinemaId.ToString(), _movieId.ToString());

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.Count());
        //    Assert.IsTrue(result.Any(s => s.Contains("18:00")));
        //    Assert.IsTrue(result.Any(s => s.Contains("21:00")));

        //    _cinemaMovieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        //}

        [Test]
        public async Task GetAllProjectionShowTimesAsync_Should_ReturnEmpty_WhenInvalidIds()
        {
            var mockQueryable = _projections.BuildMock();
            _cinemaMovieRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

            var result = await _projectionService.GetAllProjectionShowTimesAsync("invalid", "invalid");

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _cinemaMovieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion

        #region GetAvailableTickets

        [Test]
        public async Task GetAvailableTickets_Should_Return_CorrectNumber_WhenProjectionExists()
        {
            var mockQueryable = _projections.BuildMock();
            _cinemaMovieRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

            var showtimeStr = _projections[0].ShowTime.ToString();
            var result = await _projectionService.GetAvailableTickets(_cinemaId.ToString(), _movieId.ToString(), showtimeStr);

            Assert.AreEqual(50, result);
            _cinemaMovieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAvailableTickets_Should_ReturnZero_WhenProjectionDoesNotExist()
        {
            var mockQueryable = _projections.BuildMock();
            _cinemaMovieRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

            var result = await _projectionService.GetAvailableTickets(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2026-02-17T00:00:00");

            Assert.AreEqual(0, result);
            _cinemaMovieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAvailableTickets_Should_ReturnZero_WhenAnyParameterIsNullOrWhitespace()
        {
            var result1 = await _projectionService.GetAvailableTickets(null, "movie", "showtime");
            var result2 = await _projectionService.GetAvailableTickets("cinema", null, "showtime");
            var result3 = await _projectionService.GetAvailableTickets("cinema", "movie", null);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
        }

        #endregion

        #region PurchaseTickets

        [Test]
        public void PurchaseTickets_Should_ThrowNotImplementedException()
        {
            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await _projectionService.PurchaseTickets(_cinemaId.ToString(), _movieId.ToString(), 1, DateTime.Now.ToString()));
        }

        #endregion
    }
}