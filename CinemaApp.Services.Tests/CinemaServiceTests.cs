using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Cinema;
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
    public class CinemaServiceTests
    {
        private Mock<ICinemaRepository> _cinemaRepoMock;
        private CinemaService _cinemaService;

        private Cinema _testCinema;
        private Guid _cinemaId;

        [SetUp]
        public void SetUp()
        {
            _cinemaRepoMock = new Mock<ICinemaRepository>(MockBehavior.Strict);
            _cinemaService = new CinemaService(_cinemaRepoMock.Object);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _cinemaId = Guid.NewGuid();

            var movie1 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie One",
                Director = "Director One",
                ImageUrl = "img1.jpg",
                Duration = 120
            };

            var movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie Two",
                Director = "Director Two",
                ImageUrl = "img2.jpg",
                Duration = 90
            };

            _testCinema = new Cinema
            {
                Id = _cinemaId,
                Name = "Test Cinema",
                Location = "Test Location",
                CinemaMovies = new List<CinemaMovie>
                {
                    new CinemaMovie { Movie = movie1 },
                    new CinemaMovie { Movie = movie2 }
                }
            };
        }

        #region GetProgramAsync Tests

        [Test]
        public async Task GetProgramAsync_ReturnsNull_When_CinemaIdIsNull()
        {
            var result = await _cinemaService.GetProgramAsync(null);
            Assert.IsNull(result);
            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetProgramAsync_ReturnsNull_When_CinemaIdIsWhitespace()
        {
            var result = await _cinemaService.GetProgramAsync("   ");
            Assert.IsNull(result);
            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetProgramAsync_ReturnsNull_When_CinemaIdIsInvalidGuid()
        {
            var result = await _cinemaService.GetProgramAsync("invalid-guid");
            Assert.IsNull(result);
            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetProgramAsync_ReturnsViewModel_When_CinemaExists()
        {
            var cinemasMock = new List<Cinema> { _testCinema }.BuildMock();
            _cinemaRepoMock.Setup(r => r.GetAllAttached()).Returns(cinemasMock);

            var result = await _cinemaService.GetProgramAsync(_cinemaId.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual(_testCinema.Id.ToString(), result.CinemaId);
            Assert.AreEqual(_testCinema.Name, result.CinemaName);
            Assert.AreEqual($"{_testCinema.Name} - {_testCinema.Location}", result.CinemaData);
            Assert.AreEqual(2, result.Movies.Count());

            Assert.IsTrue(result.Movies.Any(m => m.Title == "Movie One" && m.Director == "Director One"));
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Movie Two" && m.Director == "Director Two"));

            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion

        #region GetUserCinemasAsync Tests

        [Test]
        public async Task GetUserCinemasAsync_ReturnsAllCinemas()
        {
            var cinemasMock = new List<Cinema> { _testCinema }.BuildMock();
            _cinemaRepoMock.Setup(r => r.GetAllAttached()).Returns(cinemasMock);

            var result = (await _cinemaService.GetUserCinemasAsync()).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(_testCinema.Id.ToString(), result[0].Id);
            Assert.AreEqual(_testCinema.Name, result[0].Name);
            Assert.AreEqual(_testCinema.Location, result[0].Location);

            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion

        #region GetCinemaDetailsAsync Tests

        [Test]
        public async Task GetCinemaDetailsAsync_ReturnsNull_When_CinemaIdIsNull()
        {
            var result = await _cinemaService.GetCinemaDetailsAsync(null);
            Assert.IsNull(result);
            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetCinemaDetailsAsync_ReturnsNull_When_CinemaIdIsInvalidGuid()
        {
            var result = await _cinemaService.GetCinemaDetailsAsync("invalid-guid");
            Assert.IsNull(result);
            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetCinemaDetailsAsync_ReturnsViewModel_When_CinemaExists()
        {
            var cinemasMock = new List<Cinema> { _testCinema }.BuildMock();
            _cinemaRepoMock.Setup(r => r.GetAllAttached()).Returns(cinemasMock);

            var result = await _cinemaService.GetCinemaDetailsAsync(_cinemaId.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual(_testCinema.Name, result.Name);
            Assert.AreEqual(_testCinema.Location, result.Location);
            Assert.AreEqual(2, result.Movies.Count());
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Movie One"));
            Assert.IsTrue(result.Movies.Any(m => m.Title == "Movie Two"));

            _cinemaRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion
    }
}