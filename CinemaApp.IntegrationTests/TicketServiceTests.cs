using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Implementations;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Ticket;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.IntegrationTests
{
    [TestFixture]
    public class TicketServiceTests
    {
        private CinemaAppDbContext _context;
        private ITicketRepository _ticketRepository;
        private ICinemaMovieRepository _cinemaMovieRepository;
        private TicketService _service;

        private Guid _userId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CinemaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CinemaAppDbContext(options);
            _ticketRepository = new TicketRepository(_context);
            _cinemaMovieRepository = new CinemaMovieRepository(_context);
            _service = new TicketService(_ticketRepository, _cinemaMovieRepository);

            _userId = Guid.NewGuid();

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var cinema = new Cinema
            {
                Id = Guid.NewGuid(),
                Name = "Arena",
                Location = "Sofia"
            };

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Inception",
                Genre = "Sci-Fi",
                ReleaseDate = new DateTime(2010, 7, 16),
                Director = "Nolan",
                Duration = 148,
                Description = "A thief who steals corporate secrets.",
                ImageUrl = "/images/inception.jpg"
            };

            var projection = new CinemaMovie
            {
                Id = Guid.NewGuid(),
                CinemaId = cinema.Id,
                MovieId = movie.Id,
                ShowTime = new DateTime(2026, 2, 17, 18, 0, 0),
                AvailableTickets = 50,
                Cinema = cinema,
                Movie = movie
            };

            _context.Cinemas.Add(cinema);
            _context.Movies.Add(movie);
            _context.CinemaMovies.Add(projection);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        // -------------------- GetUserTicketsAsync --------------------
        [Test]
        public async Task GetUserTicketsAsync_ReturnsEmpty_WhenUserHasNoTickets()
        {
            var result = await _service.GetUserTicketsAsync(_userId);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserTicketsAsync_ReturnsTickets_WhenUserHasTickets()
        {
            var projection = _context.CinemaMovies.First();

            // Add a ticket for user
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                CinemaMovieProjections = projection,
                CinemaMovieId = projection.Id,
                UserId = _userId,
                Quantity = 2,
                PricePerTicket = 10.0m
            };

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            var result = await _service.GetUserTicketsAsync(_userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Inception", result.First().MovieTitle);
            Assert.AreEqual(2, result.First().TicketCount);
            Assert.AreEqual(20.0m, result.First().TotalPrice);
        }

        // -------------------- PurchaseTickets --------------------
        [Test]
        public async Task PurchaseTickets_AddsNewTicket_WhenUserHasNoExistingTicket()
        {
            var projection = _context.CinemaMovies.First();
            var result = await _service.PurchaseTickets(
                projection.CinemaId.ToString(),
                projection.MovieId.ToString(),
                3,
                projection.ShowTime.ToString(),
                _userId
            );

            Assert.IsTrue(result);

            var ticket = _context.Tickets.FirstOrDefault(t => t.UserId == _userId);
            Assert.IsNotNull(ticket);
            Assert.AreEqual(3, ticket.Quantity);

            var updatedProjection = _context.CinemaMovies.Find(projection.Id);
            Assert.AreEqual(47, updatedProjection.AvailableTickets);
        }

        [Test]
        public async Task PurchaseTickets_UpdatesExistingTicket_WhenUserAlreadyHasTicket()
        {
            var projection = _context.CinemaMovies.First();

            // Seed existing ticket
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                CinemaMovieProjections = projection,
                CinemaMovieId = projection.Id,
                UserId = _userId,
                Quantity = 2,
                PricePerTicket = 10.0m
            };
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            var result = await _service.PurchaseTickets(
                projection.CinemaId.ToString(),
                projection.MovieId.ToString(),
                3,
                projection.ShowTime.ToString(),
                _userId
            );

            Assert.IsTrue(result);

            var updatedTicket = _context.Tickets.FirstOrDefault(t => t.UserId == _userId);
            Assert.IsNotNull(updatedTicket);
            Assert.AreEqual(5, updatedTicket.Quantity);

            var updatedProjection = _context.CinemaMovies.Find(projection.Id);
            Assert.AreEqual(47, updatedProjection.AvailableTickets);
        }

        [Test]
        public async Task PurchaseTickets_ReturnsFalse_WhenParametersAreInvalid()
        {
            var projection = _context.CinemaMovies.First();

            var result1 = await _service.PurchaseTickets(null, projection.MovieId.ToString(), 1, projection.ShowTime.ToString(), _userId);
            var result2 = await _service.PurchaseTickets(projection.CinemaId.ToString(), null, 1, projection.ShowTime.ToString(), _userId);
            var result3 = await _service.PurchaseTickets(projection.CinemaId.ToString(), projection.MovieId.ToString(), 0, projection.ShowTime.ToString(), _userId);
            var result4 = await _service.PurchaseTickets(projection.CinemaId.ToString(), projection.MovieId.ToString(), 1, null, _userId);
            var result5 = await _service.PurchaseTickets(projection.CinemaId.ToString(), projection.MovieId.ToString(), 1, projection.ShowTime.ToString(), Guid.Empty);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsFalse(result5);
        }
    }
}
