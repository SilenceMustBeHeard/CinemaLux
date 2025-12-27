<<<<<<< HEAD
namespace CinemaApp.Web
{
    using CinemaApp.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<CinemaAppDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CinemaAppDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
=======
using CinemaApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Data.Seeding.DbSeeder;

namespace CinemaApp.Web
{
    using CinemaApp.Data;
    using CinemaApp.Data.Seeding;
    using CinemaApp.Services.Core;
    using CinemaApp.Services.Core.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add DbContext
            builder.Services.AddDbContext<CinemaAppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllersWithViews()
       .AddRazorRuntimeCompilation();


            // Add Identity
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {  // for production use the following options

                //options.SignIn.RequireConfirmedAccount = true;
                //options.SignIn.RequireConfirmedEmail = true;
                //options.SignIn.RequireConfirmedPhoneNumber = true;

                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequiredUniqueChars = 4;
                //options.Password.RequiredLength = 15;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); 
                //options.Lockout.MaxFailedAccessAttempts = 3; 
                //options.Lockout.AllowedForNewUsers = true; 

                // for testing purposes only
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
            .AddEntityFrameworkStores<CinemaAppDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IMovieService, MovieService>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // Migrations + seed
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CinemaAppDbContext>();

               
                await dbContext.Database.MigrateAsync();

                // Seed movies 
                if (!await dbContext.Movies.AnyAsync())
                {
                    await DbSeeder.SeedMoviesAsync(dbContext);
                }
            }

            // Configure middleware
>>>>>>> 2affda0 (Add project files.)
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
<<<<<<< HEAD
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
=======
>>>>>>> 2affda0 (Add project files.)
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
<<<<<<< HEAD

            app.UseRouting();

=======
            app.UseRouting();
>>>>>>> 2affda0 (Add project files.)
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

<<<<<<< HEAD
            app.Run();
=======
            await app.RunAsync();


>>>>>>> 2affda0 (Add project files.)
        }
    }
}
