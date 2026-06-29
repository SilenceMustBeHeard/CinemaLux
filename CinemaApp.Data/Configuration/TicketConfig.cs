using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.PricePerTicket)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(t => t.Quantity)
                .IsRequired();

            builder.HasOne(t => t.CinemaMovieProjections)
                .WithMany(cm => cm.Tickets)
                .HasForeignKey(t => t.CinemaMovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}