using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync();

        Task AddMovieAsync(MovieFormModelCreate model);
        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string id);

        Task<MovieFormModelEdit?> GetMovieForEditByIdAsync(string id);

        Task EditMovieAsync(MovieFormModelEdit model);


        Task <MovieFormModelDelete?> GetMovieForDeleteByIdAsync(string id);

        Task<bool> SoftDeleteMovieAsync(string? id);
        Task<bool> DeleteMovieAsync(string? id);

    }
}

