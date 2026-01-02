using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    public class AppUserMovieConfig : IEntityTypeConfiguration<AppUserMovie>
    {
        public void Configure(EntityTypeBuilder<AppUserMovie> builder)
        {
            // Composite Key
            builder.HasKey(um => new { um.AppUserId, um.MovieId });

            // Navigation props
            builder.HasOne(um => um.AppUser)
                   .WithMany(u => u.WatchList)
                   .HasForeignKey(um => um.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(um => um.Movie)
                   .WithMany(m => m.AppUserMovies)
                   .HasForeignKey(um => um.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Default values
            builder.Property(um => um.IsActive)
                   .HasDefaultValue(true);

            builder.Property(um => um.AddedOn)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(um => um.IsLiked)
                   .HasDefaultValue(false);
        }
    }
}
