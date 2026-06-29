using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;

namespace CinemaApp.Data.Repository.Implementations
{
    public class TicketRepository :
        BaseRepository<Ticket, Guid>, ITicketRepository
    {
        public TicketRepository(CinemaAppDbContext context)
            : base(context)
        {
        }
    }
}