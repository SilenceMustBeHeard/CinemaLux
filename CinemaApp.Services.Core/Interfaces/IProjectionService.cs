namespace CinemaApp.Services.Core.Interfaces
{
    public interface IProjectionService
    {
        Task<IEnumerable<string>> GetAllProjectionShowTimesAsync(string? cinemaId, string? movieId);

        Task<int> GetAvailableTickets(string cinemaId, string movieId, string showtime);

        Task<bool> PurchaseTickets(string cinemaId, string movieId, int quantity, string showtime);
    }
}