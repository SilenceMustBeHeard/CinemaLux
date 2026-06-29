using CinemaApp.Data.Models;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface IManagerRepository :
        IRepository<Manager, Guid>, IRepositoryAsync<Manager, Guid>
    {
    }
}