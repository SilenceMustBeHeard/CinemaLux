using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Implementations;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Seeding;
using CinemaApp.Services.Core;
using CinemaApp.Services.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace CinemaApp.Web
{


    public class Program
    {
        public static async Task Main(string[] args)
        {
           

            var builder = WebApplication.CreateBuilder(args);

            // ====== Connection string ======
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // ====== Add DbContext ======
            builder.Services.AddDbContext<CinemaAppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // ====== Add Identity ======
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // ====== Identity options ======
                // ====== Less restrictive options ======
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;



                // ====== More restrictive options (commented out) ======

                //options.SignIn.RequireConfirmedAccount = true;
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireUppercase = true;

                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequiredLength = 16;
            })
            .AddEntityFrameworkStores<CinemaAppDbContext>()
            .AddDefaultTokenProviders();

            // ====== Add services ======
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();


            builder.Services.AddScoped<IWatchListRepository, WatchListRepository>();
            builder.Services.AddScoped<IWatchListService, WatchListService>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 

            var app = builder.Build();

            // ====== Seed database ======
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CinemaAppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await dbContext.Database.MigrateAsync();

                // Seed Admin
                await IdentitySeeder.SeedAdminAsync(userManager, roleManager);

                // Seed Movies
                if (!await dbContext.Movies.AnyAsync())
                {
                    await DbSeeder.SeedMoviesAsync(dbContext);
                }
            }

            // ====== Middleware ======
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // ====== Routing ======
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            await app.RunAsync();

        }
    }
}
