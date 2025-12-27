using CinemaApp.Data.Models;
using CinemaApp.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
    }
}
