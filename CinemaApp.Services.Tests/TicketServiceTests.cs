using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Ticket;
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
    public class TicketServiceTests
    {
        private Mock<ITicketRepository> _ticketRepoMock;
        private Mock<ICinemaMovieRepository> _cinemaMovieRepoMock;
        private TicketService _ticketService;

        private Guid _userId;
        private Guid _cinemaId;
        private Guid _movieId;
        private List<CinemaMovie> _projections;
        private List<Ticket> _tickets;

        [SetUp]
        public void SetUp()
        {
            _ticketRepoMock = new Mock<ITicketRepository>(MockBehavior.Strict);
            _cinemaMovieRepoMock = new Mock<ICinemaMovieRepository>(MockBehavior.Strict);
            _ticketService = new TicketService(_ticketRepoMock.Object, _cinemaMovieRepoMock.Object);

            SeedTestData();
        }

        private void SeedTestData()
        {
            _userId = Guid.NewGuid();
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
                    AvailableTickets = 50,
                    Movie = new Movie { Id = _movieId, Title = "Test Movie", ImageUrl = "img.jpg" },
                    Cinema = new Cinema { Id = _cinemaId, Name = "Test Cinema" }
                }
            };

            _tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = Guid.NewGuid(),
                    UserId = _userId,
                    CinemaMovieProjections = _projections[0],
                    Quantity = 2,
                    PricePerTicket = 10.0m
                }
            };
        }

        #region GetUserTicketsAsync

        [Test]
        public async Task GetUserTicketsAsync_Should_ReturnTickets_WhenUserHasTickets()
        {
            var mockQueryable = _tickets.BuildMock();
            _ticketRepoMock.Setup(r => r.GetAllAttached()).Returns(mockQueryable);

            var result = await _ticketService.GetUserTicketsAsync(_userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Test Movie", result.First().MovieTitle);
            _ticketRepoMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetUserTicketsAsync_Should_ReturnEmpty_WhenUserIdIsNull()
        {
            var result = await _ticketService.GetUserTicketsAsync(null);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        #endregion

        #region PurchaseTickets

        [Test]
        public async Task PurchaseTickets_Should_AddTicket_WhenNoExistingTicket()
        {
            var mockProjections = _projections.BuildMock();
            _cinemaMovieRepoMock.Setup(r => r.GetAllAttached()).Returns(mockProjections);

            var emptyTickets = new List<Ticket>().BuildMock();
            _ticketRepoMock.Setup(r => r.GetAllAttached()).Returns(emptyTickets);

            _ticketRepoMock.Setup(r => r.AddAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);
            _cinemaMovieRepoMock.Setup(r => r.UpdateAsync(It.IsAny<CinemaMovie>())).ReturnsAsync(true);

            var showtimeStr = _projections[0].ShowTime.ToString();
            var result = await _ticketService.PurchaseTickets(_cinemaId.ToString(), _movieId.ToString(), 3, showtimeStr, _userId);

            Assert.IsTrue(result);
            Assert.AreEqual(47, _projections[0].AvailableTickets);

            _ticketRepoMock.Verify(r => r.AddAsync(It.IsAny<Ticket>()), Times.Once);
            _cinemaMovieRepoMock.Verify(r => r.UpdateAsync(It.IsAny<CinemaMovie>()), Times.Once);
        }


        // TO DO : Fix this test after implementing the update logic in PurchaseTickets method

        //[Test]
        //public async Task PurchaseTickets_Should_UpdateTicket_WhenExistingTicket()
        //{
        //    var mockProjections = _projections.BuildMock();
        //    _cinemaMovieRepoMock.Setup(r => r.GetAllAttached()).Returns(mockProjections);

        //    var mockTickets = _tickets.BuildMock();
        //    _ticketRepoMock.Setup(r => r.GetAllAttached()).Returns(mockTickets);

        //    _ticketRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);
        //    _cinemaMovieRepoMock.Setup(r => r.UpdateAsync(It.IsAny<CinemaMovie>())).ReturnsAsync(true);

        //    var showtimeStr = _projections[0].ShowTime.ToString();
        //    var result = await _ticketService.PurchaseTickets(_cinemaId.ToString(), _movieId.ToString(), 3, showtimeStr, _userId);

        //    Assert.IsTrue(result);
        //    Assert.AreEqual(47, _projections[0].AvailableTickets);
        //    Assert.AreEqual(5, _tickets[0].Quantity);

        //    _ticketRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Ticket>()), Times.Once);
        //    _cinemaMovieRepoMock.Verify(r => r.UpdateAsync(It.IsAny<CinemaMovie>()), Times.Once);
        //}

        [Test]
        public async Task PurchaseTickets_Should_ReturnFalse_WhenInvalidParameters()
        {
            var result = await _ticketService.PurchaseTickets(null, null, 0, null, Guid.Empty);
            Assert.IsFalse(result);
        }

        #endregion
    }
}