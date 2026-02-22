using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
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
    public class MovieServiceTests
    {
        private Mock<IMovieRepository> _movieRepoMock;
        private MovieService _movieService;

        private Movie _movie1;
        private Movie _movie2;
        private Guid _movieId1;
        private Guid _movieId2;

        [SetUp]
        public void SetUp()
        {
            _movieRepoMock = new Mock<IMovieRepository>(MockBehavior.Strict);
            _movieService = new MovieService(_movieRepoMock.Object);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _movieId1 = Guid.NewGuid();
            _movieId2 = Guid.NewGuid();

            _movie1 = new Movie
            {
                Id = _movieId1,
                Title = "Movie One",
                Genre = "Action",
                ReleaseDate = new DateTime(2020, 1, 1),
                Director = "Director One",
                Duration = 120,
                Description = "Desc1",
                ImageUrl = "img1.jpg",
                TrailerUrl = "trailer1.mp4",
                IsDeleted = false
            };

            _movie2 = new Movie
            {
                Id = _movieId2,
                Title = "Movie Two",
                Genre = "Comedy",
                ReleaseDate = new DateTime(2021, 2, 2),
                Director = "Director Two",
                Duration = 90,
                Description = "Desc2",
                ImageUrl = "img2.jpg",
                TrailerUrl = "trailer2.mp4",
                IsDeleted = false
            };
        }

        #region AddMovieAsync

        [Test]
        public async Task AddMovieAsync_Should_Call_AddAsync_OnRepo()
        {
            var model = new MovieFormModelCreate
            {
                Title = "New Movie",
                Genre = "Drama",
                ReleaseDate = DateTime.Today,
                Director = "Dir",
                Duration = 100,
                Description = "Desc",
                ImageUrl = "img.jpg",
                TrailerUrl = "trailer.mp4"
            };

            _movieRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Movie>()))
                .Returns(Task.CompletedTask);

            await _movieService.AddMovieAsync(model);

            _movieRepoMock.Verify(r => r.AddAsync(It.Is<Movie>(m => m.Title == "New Movie")), Times.Once);
        }

        #endregion

        #region GetAllMoviesAsync

        [Test]
        public async Task GetAllMoviesAsync_Should_Return_NonDeletedMovies()
        {
            var moviesMock = new List<Movie> { _movie1, _movie2 }.BuildMock();
            _movieRepoMock.Setup(r => r.GetAllAttached()).Returns(moviesMock);

            var result = (await _movieService.GetAllMoviesAsync()).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(m => m.Title == "Movie One"));
            Assert.IsTrue(result.Any(m => m.Title == "Movie Two"));

            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion

        #region GetMovieDetailsByIdAsync

        [Test]
        public async Task GetMovieDetailsByIdAsync_Should_Return_Null_When_InvalidGuid()
        {
            var result = await _movieService.GetMovieDetailsByIdAsync("invalid", null);
            Assert.IsNull(result);
            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetMovieDetailsByIdAsync_Should_Return_Movie_When_Valid()
        {
            var moviesMock = new List<Movie> { _movie1 }.BuildMock();
            _movieRepoMock.Setup(r => r.GetAllAttached()).Returns(moviesMock);

            var result = await _movieService.GetMovieDetailsByIdAsync(_movieId1.ToString(), null);

            Assert.IsNotNull(result);
            Assert.AreEqual(_movie1.Title, result.Title);
            Assert.AreEqual(_movie1.Genre, result.Genre);
            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion

        #region GetMovieForEditByIdAsync

        [Test]
        public async Task GetMovieForEditByIdAsync_Should_Return_Null_When_InvalidGuid()
        {
            var result = await _movieService.GetMovieForEditByIdAsync("invalid");
            Assert.IsNull(result);
            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetMovieForEditByIdAsync_Should_Return_EditModel_When_Valid()
        {
            var moviesMock = new List<Movie> { _movie1 }.BuildMock();
            _movieRepoMock.Setup(r => r.GetAllAttached()).Returns(moviesMock);

            var result = await _movieService.GetMovieForEditByIdAsync(_movieId1.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual(_movie1.Title, result.Title);
            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion

        #region EditMovieAsync

        //[Test]
        //public async Task EditMovieAsync_Should_Update_Movie()
        //{
        //    var model = new MovieFormModelEdit
        //    {
        //        Id = _movieId1.ToString(),
        //        Title = "Edited",
        //        Genre = "EditedGenre",
        //        ReleaseDate = _movie1.ReleaseDate,
        //        Director = "EditedDir",
        //        Duration = _movie1.Duration,
        //        Description = "EditedDesc",
        //        ImageUrl = "new.jpg",
        //        TrailerUrl = "newtrailer.mp4"
        //    };

        //    _movieRepoMock.Setup(r => r.GetByIdAsync(_movieId1)).ReturnsAsync(_movie1);
        //    // TO DO : Fix - UpdateAsync should return the updated movie, not void
        //    _movieRepoMock.Setup(r => r.UpdateAsync(_movie1)).Returns(Task.CompletedTask);

        //    await _movieService.EditMovieAsync(model);

        //    Assert.AreEqual("Edited", _movie1.Title);
        //    Assert.AreEqual("EditedGenre", _movie1.Genre);
        //    Assert.AreEqual("EditedDir", _movie1.Director);

        //    _movieRepoMock.Verify(r => r.GetByIdAsync(_movieId1), Times.Once);
        //    _movieRepoMock.Verify(r => r.UpdateAsync(_movie1), Times.Once);
        //}

        #endregion

        #region SoftDeleteMovieAsync

        [Test]
        public async Task SoftDeleteMovieAsync_Should_ReturnFalse_When_InvalidGuid()
        {
            var result = await _movieService.SoftDeleteMovieAsync("invalid");
            Assert.IsFalse(result);
            _movieRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task SoftDeleteMovieAsync_Should_ReturnFalse_When_AlreadyDeleted()
        {
            _movie1.IsDeleted = true;
            _movieRepoMock.Setup(r => r.GetByIdAsync(_movieId1)).ReturnsAsync(_movie1);

            var result = await _movieService.SoftDeleteMovieAsync(_movieId1.ToString());
            Assert.IsFalse(result);
            _movieRepoMock.Verify(r => r.GetByIdAsync(_movieId1), Times.Once);
        }

        [Test]
        public async Task SoftDeleteMovieAsync_Should_Call_DeleteAsync_When_Valid()
        {
            _movieRepoMock.Setup(r => r.GetByIdAsync(_movieId1)).ReturnsAsync(_movie1);
            _movieRepoMock.Setup(r => r.DeleteAsync(_movie1)).ReturnsAsync(true);

            var result = await _movieService.SoftDeleteMovieAsync(_movieId1.ToString());
            Assert.IsTrue(result);

            _movieRepoMock.Verify(r => r.GetByIdAsync(_movieId1), Times.Once);
            _movieRepoMock.Verify(r => r.DeleteAsync(_movie1), Times.Once);
        }

        #endregion

        #region DeleteMovieAsync

        [Test]
        public async Task DeleteMovieAsync_Should_ReturnFalse_When_InvalidGuid()
        {
            var result = await _movieService.DeleteMovieAsync("invalid");
            Assert.IsFalse(result);
            _movieRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task DeleteMovieAsync_Should_ReturnFalse_When_NotSoftDeleted()
        {
            _movieRepoMock.Setup(r => r.GetByIdAsync(_movieId1)).ReturnsAsync(_movie1);
            var result = await _movieService.DeleteMovieAsync(_movieId1.ToString());
            Assert.IsFalse(result);
            _movieRepoMock.Verify(r => r.GetByIdAsync(_movieId1), Times.Once);
        }

        [Test]
        public async Task DeleteMovieAsync_Should_Call_HardDeleteAsync_When_SoftDeleted()
        {
            _movie1.IsDeleted = true;
            _movieRepoMock.Setup(r => r.GetByIdAsync(_movieId1)).ReturnsAsync(_movie1);
            _movieRepoMock.Setup(r => r.HardDeleteAsync(_movie1)).ReturnsAsync(true);

            var result = await _movieService.DeleteMovieAsync(_movieId1.ToString());
            Assert.IsTrue(result);

            _movieRepoMock.Verify(r => r.GetByIdAsync(_movieId1), Times.Once);
            _movieRepoMock.Verify(r => r.HardDeleteAsync(_movie1), Times.Once);
        }

        #endregion

        #region GetMovieForDeleteByIdAsync

        [Test]
        public async Task GetMovieForDeleteByIdAsync_Should_Return_Null_When_InvalidGuid()
        {
            var result = await _movieService.GetMovieForDeleteByIdAsync("invalid");
            Assert.IsNull(result);
            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetMovieForDeleteByIdAsync_Should_Return_Model_When_Valid()
        {
            var moviesMock = new List<Movie> { _movie1 }.BuildMock();
            _movieRepoMock.Setup(r => r.GetAllAttached()).Returns(moviesMock);

            var result = await _movieService.GetMovieForDeleteByIdAsync(_movieId1.ToString());
            Assert.IsNotNull(result);
            Assert.AreEqual(_movie1.Id.ToString(), result.Id);
            Assert.AreEqual(_movie1.Title, result.Title);

            _movieRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        #endregion
    }
}