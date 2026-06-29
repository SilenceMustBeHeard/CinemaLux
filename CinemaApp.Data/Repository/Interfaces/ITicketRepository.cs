using CinemaApp.Data.Models;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface ITicketRepository :
        IRepository<Ticket, Guid>, IRepositoryAsync<Ticket, Guid>
    {
    }
}