namespace CinemaApp.Data.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public Guid CinemaMovieId { get; set; }
        public virtual CinemaMovie CinemaMovieProjections { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal PricePerTicket { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}