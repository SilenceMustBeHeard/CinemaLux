using CinemaApp.Data.Models;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface ICinemaRepository :
        IRepository<Cinema, Guid>, IRepositoryAsync<Cinema, Guid>
    {
    }
}