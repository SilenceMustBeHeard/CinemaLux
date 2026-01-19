using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketIndexViewModel>> GetUserTicketsAsync(Guid? userId);


        Task<bool> PurchaseTickets(string cinemaId, string movieId, int quantity, string showtime, Guid userId);
    }
}
