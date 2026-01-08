

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data.Seeding
{
    public static class DbSeeder
    {
        public static async Task SeedMoviesAsync(CinemaAppDbContext context)
        {
            if (await context.Movies.AnyAsync())
                return;


            var jsonPath = Path.Combine(
      Directory.GetCurrentDirectory(),
      "wwwroot",
      "data",
      "movies.json"
  );

            if (!File.Exists(jsonPath))
            {
                throw new Exception($"movies.json NOT FOUND at: {jsonPath}");
            }


            var json = await File.ReadAllTextAsync(jsonPath);
            var movies = JsonSerializer.Deserialize<List<Movie>>(json);

            if (movies != null && movies.Count > 0)
            {
                context.Movies.AddRange(movies);
                await context.SaveChangesAsync();
            }
        }





        public static async Task SeedCinemasAsync(CinemaAppDbContext context)
        {
            
            if (await context.Cinemas.AnyAsync())
            {
                return;
            }

            var jsonPath = Path.Combine(
     Directory.GetCurrentDirectory(),
     "wwwroot",
     "data",
               "cinemas.json");

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException("cinemas.json not found", jsonPath);
            }

            string json = await File.ReadAllTextAsync(jsonPath);

            var cinemaDtos = JsonSerializer.Deserialize<List<CinemaSeedDto>>(json)
                             ?? new List<CinemaSeedDto>();

            var cinemas = cinemaDtos.Select(c => new Cinema
            {
                Id = Guid.NewGuid(),
                Name = c.Name,
                Location = c.Location,
                IsDeleted = false
            });

            await context.Cinemas.AddRangeAsync(cinemas);
            await context.SaveChangesAsync();
        }

        private class CinemaSeedDto
        {
            public string Name { get; set; } = null!;
            public string Location { get; set; } = null!;
        }
    }
}

