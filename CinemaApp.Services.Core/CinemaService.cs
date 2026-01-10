
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Cinema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemaRepository _cinemaRepo;

        public CinemaService(ICinemaRepository cinemaRepo) => _cinemaRepo = cinemaRepo;

        public async Task<CinemaProgramViewModel?> GetProgramAsync(string? cinemaId)
        {
            if (string.IsNullOrWhiteSpace(cinemaId))
            {
                return null;
            }

            if (!Guid.TryParse(cinemaId, out Guid cinemaGuid))
            {
                return null;
            }

            var cinema = await _cinemaRepo
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                    .ThenInclude(cm => cm.Movie)
                .SingleOrDefaultAsync(c => c.Id == cinemaGuid);

            if (cinema == null)
            {
                return null;
            }

            return new CinemaProgramViewModel
            {
                CinemaId = cinema.Id.ToString(),
                CinemaName = cinema.Name,
                CinemaData = $"{cinema.Name} - {cinema.Location}",
                Movies = cinema.CinemaMovies
                    .Select(cm => cm.Movie)
                    .Select(m => new CinemaProgramMovieViewModel
                    {
                        MovieId = m.Id.ToString(),
                        Title = m.Title,
                        ImageUrl = m.ImageUrl ?? $"/images/",
                        Director = m.Director
                    })
                    .ToArray()
            };
        }











        public async Task<IEnumerable<UsersCinemaIndexViewModel>> GetUserCinemasAsync()

              => await _cinemaRepo.GetAllAttached()
                .Select(c => new UsersCinemaIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location
                })
                .ToArrayAsync();






        public async Task<CinemaDetailsViewModel?> GetCinemaDetailsAsync(string? cinemaId)
        {
            if (string.IsNullOrWhiteSpace(cinemaId))
            {
                return null;
            }

            var cinema = await _cinemaRepo
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                    .ThenInclude(cm => cm.Movie)
                .SingleOrDefaultAsync(c => c.Id.ToString() == cinemaId);

            if (cinema == null)
            {
                return null;
            }

            return new CinemaDetailsViewModel
            {
                Name = cinema.Name,
                Location = cinema.Location,
                Movies = cinema.CinemaMovies
                    .Select(cm => cm.Movie)
                    .Select(m => new CinemaDetailsMovieViewModel
                    {
                        Title = m.Title,
                        Duration = m.Duration.ToString()
                    })
                    .ToArray()
            };
        }

    }
}
