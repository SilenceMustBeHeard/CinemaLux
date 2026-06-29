using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;

namespace CinemaApp.Data.Repository.Implementations
{
    public class ManagerRepository : BaseRepository<Manager, Guid>, IManagerRepository
    {
        public ManagerRepository(CinemaAppDbContext context)
            : base(context)
        {
        }
    }
}