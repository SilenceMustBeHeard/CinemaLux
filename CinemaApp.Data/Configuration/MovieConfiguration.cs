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
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                   .IsRequired()
                   .HasMaxLength(200);
                  

            builder.Property(m => m.Genre)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(m => m.ReleaseDate)
                   .IsRequired();

            builder.Property(m => m.Director)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(m => m.Duration)
                   .IsRequired();

            builder.Property(m => m.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(m => m.ImageUrl)
                   .IsRequired(false);

          builder.Property(m=> m.TrailerUrl)
                   .IsRequired(false);


            builder.Property(m => m.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
