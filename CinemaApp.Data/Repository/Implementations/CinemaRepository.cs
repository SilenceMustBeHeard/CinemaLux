using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;

namespace CinemaApp.Data.Repository.Implementations
{
    public class CinemaRepository :
        BaseRepository<Cinema, Guid>, ICinemaRepository
    {
        public CinemaRepository(CinemaAppDbContext context)
            : base(context)
        {
        }
    }
}