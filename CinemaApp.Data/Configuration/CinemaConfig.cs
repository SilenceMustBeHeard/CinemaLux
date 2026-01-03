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
    public class CinemaConfig : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired();


            builder.Property(c => c.Location)
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.HasMany(c => c.CinemaMovies)
                .WithOne(cm => cm.Cinema)
                .HasForeignKey(cm => cm.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Tickets)
                .WithOne(t => t.Cinema)
                .HasForeignKey(t => t.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Manager)
                .WithMany(m => m.ManagedCinemas)
                .HasForeignKey(c => c.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);




            builder.HasQueryFilter(c => !c.IsDeleted );

            builder.HasIndex(c => new { c.Name, c.Location })
                .IsUnique(true);
        }
    }
}
    

