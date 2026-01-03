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
    public class ManagerConfig : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x=>x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Manager>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.UserId })
                .IsUnique();

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
