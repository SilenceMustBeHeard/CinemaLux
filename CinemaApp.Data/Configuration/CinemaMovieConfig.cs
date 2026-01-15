using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
    public class CinemaMovieConfig : IEntityTypeConfiguration<CinemaMovie>
    {
        public void Configure(EntityTypeBuilder<CinemaMovie> builder)
        {
            builder.HasKey(cm => cm.Id);

            builder.HasIndex(cm => new
            {
                cm.MovieId,
                cm.CinemaId,
                cm.ShowTime
            })
            .IsUnique();

            builder.Property(cm => cm.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(cm => cm.AvailableTickets)
                .HasDefaultValue(0);

            builder.Property(cm => cm.ShowTime)
                .IsRequired();

            builder.HasQueryFilter(cm =>
                !cm.IsDeleted &&
                !cm.Movie.IsDeleted &&
                !cm.Cinema.IsDeleted);

            builder.HasOne(cm => cm.Movie)
                .WithMany(m => m.MovieProjections)
                .HasForeignKey(cm => cm.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cm => cm.Cinema)
                .WithMany(c => c.CinemaMovies)
                .HasForeignKey(cm => cm.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
