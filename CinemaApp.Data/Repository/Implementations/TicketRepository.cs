using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Implementations
{
    public class TicketRepository:
        BaseRepository<Ticket, Guid>, ITicketRepository
    {
        protected TicketRepository(CinemaAppDbContext context)
            : base(context)
        {
        }
    }
}
