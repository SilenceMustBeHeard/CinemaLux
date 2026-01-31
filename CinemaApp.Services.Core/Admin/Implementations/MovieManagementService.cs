using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Admin.MovieManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Implementations
{
    public class MovieManagementService : MovieService, IMovieManagementService
    {

        private readonly IMovieRepository _movieRepository;

        public MovieManagementService(IMovieRepository movieRepository)
            : base(movieRepository)
        {
            _movieRepository = movieRepository;
        }



      public  async Task<IEnumerable<MovieManagementIndexViewModel>> GetAllMovieManagementAsync()
        {
            var movies = await _movieRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Select(m => new MovieManagementIndexViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    Duration = m.Duration,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate,
                    IsDeleted = m.IsDeleted
                })
                .ToArrayAsync();

            return movies;
        }








      
    }
}
