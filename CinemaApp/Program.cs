using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Data.Seeding;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.Authorization.Handlers;
using CinemaApp.Web.Authorization.Requirements;
using CinemaApp.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
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
            builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
            .AddEntityFrameworkStores<CinemaAppDbContext>()
            .AddDefaultTokenProviders();

            // ====== Add Controllers / Razor ======
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddRazorPages();

            // ====== Repositories & Services ======
            builder.Services.RegisterRepositories(typeof(IMovieRepository).Assembly);
            builder.Services.RegisterServices(typeof(IMovieService).Assembly);

            // ====== Authorization Policy ======
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerOnly", policy =>
                    policy.Requirements.Add(new ManagerRequirement()));
            });

            builder.Services.AddScoped<IAuthorizationHandler, ManagerRequirementHandler>();

            // ====== Developer Tools ======
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // ====== Seed database ======
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CinemaAppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                await dbContext.Database.MigrateAsync();

                await IdentitySeeder.SeedRolesAndUsersAsync(userManager, roleManager);

                if (!await dbContext.Movies.AnyAsync())
                {
                    await DbSeeder.SeedMoviesAsync(dbContext);
                }

                await DbSeeder.SeedCinemasAsync(dbContext);
            }

            // ====== Middleware ======
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error/NotImplemented");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
          //  app.UseManagerAccessRestriction();
            app.UseAuthorization();


            // ====== Routing ======

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area}/{controller=Home}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            await app.RunAsync();
        }
    }
}
