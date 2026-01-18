using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data.Models;

namespace CinemaApp.Services.Core.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICinemaMovieRepository _cinemaMovieRepository;

        public TicketService(ITicketRepository ticketRepository, ICinemaMovieRepository cinemaMovieRepository)
        {
            _ticketRepository = ticketRepository;
            _cinemaMovieRepository = cinemaMovieRepository;
        }



        public async Task<IEnumerable<TicketIndexViewModel>> GetUserTicketsAsync(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Enumerable.Empty<TicketIndexViewModel>();
            }

            var tickets = await _ticketRepository
                .GetAllAttached()
                .Where(t => t.UserId.ToLower() == userId.ToLower())
                .Select(t => new TicketIndexViewModel
                {
                    MovieTitle = t.CinemaMovieProjections.Movie.Title,
                    MovieImageUrl = t.CinemaMovieProjections.Movie.ImageUrl,
                    CinemaName = t.CinemaMovieProjections.Cinema.Name,
                    ShowTime = t.CinemaMovieProjections.ShowTime
                                   .ToString("dd.MM.yyyy HH:mm"),
                    TicketCount = t.Quantity,
                    TicketPrice = t.PricePerTicket,
                    TotalPrice = (t.PricePerTicket * t.Quantity)


                })
                .ToListAsync();

            return tickets;


        }

        public async Task<bool> PurchaseTickets(string cinemaId, string movieId, int quantity, string showtime, string userId)
        {
            var result = false;
            if (string.IsNullOrWhiteSpace(cinemaId)
                || string.IsNullOrWhiteSpace(movieId)
                || string.IsNullOrWhiteSpace(showtime)
                || string.IsNullOrWhiteSpace(userId)
                || quantity <= 0)
            {
                return await Task.FromResult(result);
            }
            var projection = await _cinemaMovieRepository.GetAllAttached()
                                .SingleOrDefaultAsync(cm => cm.CinemaId.ToString().ToLower() == cinemaId.ToLower()
                                && cm.MovieId.ToString().ToLower() == movieId.ToLower()
                                && cm.ShowTime.ToString() == showtime);

            if (projection != null
                || projection.AvailableTickets >= quantity)

            {

                var projectionTicket = await _ticketRepository.GetAllAttached()
                     .SingleOrDefaultAsync(cm => cm.CinemaMovieId.ToString().ToLower() == projection.Id.ToString().ToLower()
                     && cm.User.Id.ToString().ToLower() == userId.ToLower());

                if (projectionTicket != null)
                {
                    projectionTicket.Quantity += quantity;
                    await _ticketRepository.UpdateAsync(projectionTicket);
                }

                else
                {
                    var ticket = new Ticket()
                    {

                        CinemaMovieProjections = projection,
                        UserId = userId,
                        Quantity = quantity,
                        PricePerTicket = 10.0m // assumed price per ticket
                    };
                 
                    await _ticketRepository.AddAsync(ticket);
                }
                projection.AvailableTickets -= quantity;
                result = await _cinemaMovieRepository.UpdateAsync(projection);

              
            }
            return await Task.FromResult(result);


        }
    }

}
