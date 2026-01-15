using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Core.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
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



    }

}
