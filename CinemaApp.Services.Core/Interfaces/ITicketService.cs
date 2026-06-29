using CinemaApp.Web.ViewModels.Ticket;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketIndexViewModel>> GetUserTicketsAsync(Guid? userId);

        Task<bool> PurchaseTickets(string cinemaId, string movieId, int quantity, string showtime, Guid userId);
    }
}