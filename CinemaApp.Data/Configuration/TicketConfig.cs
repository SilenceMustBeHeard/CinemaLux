using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Configuration
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t=>t.UserId).IsRequired();

            builder.Property(t => t.Price)
                .HasPrecision(18, 2);

            builder.HasOne(t => t.CinemaMovieProjections)
                .WithMany(cm => cm.Tickets)
                .HasForeignKey(t => t.CinemaMovieId);

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);




        }
    }
}
