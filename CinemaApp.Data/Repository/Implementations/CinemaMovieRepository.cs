using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;

namespace CinemaApp.Data.Repository.Implementations
{
    public class CinemaMovieRepository : BaseRepository<CinemaMovie, Guid>, ICinemaMovieRepository
    {
        public CinemaMovieRepository(CinemaAppDbContext context)
            : base(context)
        {
        }
    }
}