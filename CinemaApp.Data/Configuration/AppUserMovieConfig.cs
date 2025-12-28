using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
    public class AppUserMovieConfig : IEntityTypeConfiguration<AppUserMovie>
    {
        public void Configure(EntityTypeBuilder<AppUserMovie> builder)
        {
            builder.HasKey(x => new { x.AppUserId, x.MovieId });

            builder.HasOne(x => x.AppUser)
                .WithMany(u => u.WatchList)  
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Movie)
                .WithMany(m => m.AppUserMovies)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
