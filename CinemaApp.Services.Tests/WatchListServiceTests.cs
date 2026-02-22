using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.WatchList;
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
    public class WatchListServiceTests
    {
        private Mock<IWatchlistRepository> _watchListRepoMock;
        private WatchListService _watchListService;

        private Guid _userId;
        private Guid _movieId;
        private List<AppUserMovie> _watchListEntries;

        [SetUp]
        public void SetUp()
        {
            _watchListRepoMock = new Mock<IWatchlistRepository>(MockBehavior.Strict);
            _watchListService = new WatchListService(_watchListRepoMock.Object);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _userId = Guid.NewGuid();
            _movieId = Guid.NewGuid();

            var movie = new Movie
            {
                Id = _movieId,
                Title = "Test Movie",
                Genre = "Action",
                ReleaseDate = new DateTime(2026, 2, 21),
                ImageUrl = "img.jpg",
                TrailerUrl = "trailer.mp4",
                IsDeleted = false
            };

            _watchListEntries = new List<AppUserMovie>
            {
                new AppUserMovie
                {
                    AppUserId = _userId,
                    MovieId = _movieId,
                    IsActive = true,
                    Movie = movie
                }
            };
        }

        #region GetWatchListByUserIdAsync

        [Test]
        public async Task GetWatchListByUserIdAsync_Should_ReturnWatchList()
        {
            var mockQueryable = _watchListEntries.BuildMock();
            _watchListRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

            var result = await _watchListService.GetWatchListByUserIdAsync(_userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Test Movie", result.First().Title);

            _watchListRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetWatchListByUserIdAsync_Should_ReturnEmpty_WhenNoEntries()
        {
            var mockQueryable = new List<AppUserMovie>().BuildMock();
            _watchListRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

            var result = await _watchListService.GetWatchListByUserIdAsync(_userId);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        #endregion

        #region ToggleWatchListAsync

        [Test]
        public async Task ToggleWatchListAsync_Should_AddEntry_WhenNotExists()
        {
            _watchListRepoMock.Setup(r => r.GetByCompositeKeyAsync(_userId, _movieId))
                              .ReturnsAsync((AppUserMovie?)null);

            _watchListRepoMock.Setup(r => r.AddAsync(It.IsAny<AppUserMovie>()))
                              .Returns(Task.CompletedTask);

            _watchListRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _watchListService.ToggleWatchListAsync(_userId, _movieId.ToString());

            _watchListRepoMock.Verify(r => r.AddAsync(It.Is<AppUserMovie>(x => x.AppUserId == _userId && x.MovieId == _movieId && x.IsActive)), Times.Once);
            _watchListRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ToggleWatchListAsync_Should_ToggleEntry_WhenExists()
        {
            var entry = _watchListEntries[0];
            _watchListRepoMock.Setup(r => r.GetByCompositeKeyAsync(_userId, _movieId))
                              .ReturnsAsync(entry);

            _watchListRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _watchListService.ToggleWatchListAsync(_userId, _movieId.ToString());

            Assert.IsFalse(entry.IsActive);

            _watchListRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ToggleWatchListAsync_Should_DoNothing_WhenMovieIdInvalid()
        {
            await _watchListService.ToggleWatchListAsync(_userId, "invalid-guid");

            _watchListRepoMock.Verify(r => r.GetByCompositeKeyAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _watchListRepoMock.Verify(r => r.AddAsync(It.IsAny<AppUserMovie>()), Times.Never);
            _watchListRepoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region IsMovieInWatchListAsync

        [Test]
        public async Task IsMovieInWatchListAsync_Should_ReturnTrue_WhenActive()
        {
            var entry = _watchListEntries[0];
            _watchListRepoMock.Setup(r => r.GetByCompositeKeyAsync(_userId, _movieId))
                              .ReturnsAsync(entry);

            var result = await _watchListService.IsMovieInWatchListAsync(_userId, _movieId.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsMovieInWatchListAsync_Should_ReturnFalse_WhenNotExists()
        {
            _watchListRepoMock.Setup(r => r.GetByCompositeKeyAsync(_userId, _movieId))
                              .ReturnsAsync((AppUserMovie?)null);

            var result = await _watchListService.IsMovieInWatchListAsync(_userId, _movieId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsMovieInWatchListAsync_Should_ReturnFalse_WhenMovieIdInvalid()
        {
            var result = await _watchListService.IsMovieInWatchListAsync(_userId, "invalid-guid");
            Assert.IsFalse(result);
        }

        #endregion
    }
}