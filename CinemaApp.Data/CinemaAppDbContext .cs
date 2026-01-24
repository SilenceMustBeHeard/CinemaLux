namespace CinemaApp.Data
{
    using CinemaApp.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class CinemaAppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public CinemaAppDbContext(DbContextOptions<CinemaAppDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<CinemaMovie> CinemaMovies { get; set; } = null!;
        public virtual DbSet<Cinema> Cinemas { get; set; } = null!;
        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Manager> Managers { get; set; } = null!;

        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        public virtual DbSet<AppUserMovie> AppUserMovies { get; set; } = null!;




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }

}
