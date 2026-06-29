namespace CinemaApp.Services.Core.Interfaces
{
    public interface IManagerService
    {
        Task<Guid?> GetIdByUserIdAsync(Guid? userId);

        Task<bool> ExistsByIdAsync(Guid id);

        Task<bool> ExistsByUserIdAsync(Guid userId);
    }
}