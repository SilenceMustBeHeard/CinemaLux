using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Implementations
{
    public class CinemaManagementService : ICinemaManagementService
    {
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaManagementService(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }



        public async Task<IEnumerable<CinemaManagementIndexViewModel>> GetAllCinemaManagementAsync()
        {
            IEnumerable<CinemaManagementIndexViewModel> cinemas = await _cinemaRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                //.Include(c => c.Manager)
                //.ThenInclude(m => m.User)
                .Select(c => new CinemaManagementIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    IsDeleted = c.IsDeleted,
                    ManagerName = c.Manager != null ? c.Manager.User.UserName : null

                }).ToArrayAsync();

            return cinemas;
        }

        public async Task CreateCinemaAsync(CinemaManagementCreateViewModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var cinema = new Cinema
            {
                Name = model.Name,
                Location = model.Location,
          
            };

            await _cinemaRepository.AddAsync(cinema);
        }
    }
}
