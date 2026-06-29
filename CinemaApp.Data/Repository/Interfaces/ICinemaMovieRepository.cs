using CinemaApp.Data.Models;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface ICinemaMovieRepository
        : IRepository<CinemaMovie, Guid>, IRepositoryAsync<CinemaMovie, Guid>
    {
    }
}