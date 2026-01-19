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



        public async Task<IEnumerable<TicketIndexViewModel>> GetUserTicketsAsync(Guid? userId)
        {
            if (userId == null)
            {
                return Enumerable.Empty<TicketIndexViewModel>();
            }

            var tickets = await _ticketRepository
                .GetAllAttached()
                .Where(t => t.UserId == userId)
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

        public async Task<bool> PurchaseTickets(string cinemaId, string movieId, int quantity, string showtime, Guid userId)
        {
            var result = false;
            if (string.IsNullOrWhiteSpace(cinemaId)
                || string.IsNullOrWhiteSpace(movieId)
                || string.IsNullOrWhiteSpace(showtime)
                || userId == Guid.Empty
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
                     .SingleOrDefaultAsync(cm => cm.CinemaMovieId == projection.Id
                     && cm.User.Id == userId);

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
