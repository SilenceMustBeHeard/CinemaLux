using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Admin.Interfaces;
using CinemaApp.Services.Core.Implementations;
using CinemaApp.Web.ViewModels.Admin.CinemaManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Admin.Implementations
{
    public class CinemaManagementService :CinemaService, ICinemaManagementService
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IUserService _userService;
        private readonly IManagerRepository _managerRepository;

     public CinemaManagementService(
            ICinemaRepository cinemaRepository,
            IUserService userService,
            IManagerRepository managerRepository
         )
            : base(cinemaRepository )
        {
            _cinemaRepository = cinemaRepository;
            _userService = userService;
            _managerRepository = managerRepository;
        }


        public async Task<IEnumerable<CinemaManagementIndexViewModel>> GetAllCinemaManagementAsync()
        {
            var cinemas = await _cinemaRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Include(c => c.Manager) 
                .ThenInclude(m => m.User) 
                .Select(c => new CinemaManagementIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    IsDeleted = c.IsDeleted,
                    ManagerName = c.Manager != null ? c.Manager.User.Email : null
                })
                .ToArrayAsync();

            return cinemas;
        }


        public async Task CreateCinemaAsync(CinemaManagementCreateViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Manager? manager = null;

            if (!string.IsNullOrWhiteSpace(model.ManagerEmail))
            {
                manager = await _managerRepository
                    .GetAllAttached()
                    .Include(m => m.User) 
                    .SingleOrDefaultAsync(m => m.User.Email == model.ManagerEmail);

                if (manager == null)
                    throw new InvalidOperationException("Selected manager does not exist.");
            }

            var cinema = new Cinema
            {
                Name = model.Name,
                Location = model.Location,
                ManagerId = manager?.Id
            };

            await _cinemaRepository.AddAsync(cinema);
        }

       
        public async Task<CinemaManagementEditViewModel> GetCinemaForEditAsync(string cinemaId)
        {
            if (string.IsNullOrWhiteSpace(cinemaId))
                throw new ArgumentNullException(nameof(cinemaId));

            var cinema = await _cinemaRepository
                .GetAllAttached()
                .Include(c => c.Manager) 
                .ThenInclude(m => m.User) 
                .SingleOrDefaultAsync(c => c.Id.ToString() == cinemaId);

            if (cinema == null)
                throw new InvalidOperationException("Cinema not found.");

            var model = new CinemaManagementEditViewModel
            {
                Id = cinema.Id.ToString(),
                Name = cinema.Name,
                Location = cinema.Location,
                ManagerEmail = cinema.Manager?.User.Email,
                Managers = await _userService.GetManagerEmailsAsync()
            };

            return model;
        }


        public async Task UpdateCinemaAsync(CinemaManagementEditViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

          
            var cinema = await _cinemaRepository
                .GetAllAttached()
                .Include(c => c.Manager)
                .ThenInclude(m => m.User)
                .SingleOrDefaultAsync(c => c.Id.ToString() == model.Id);

            if (cinema == null)
                throw new InvalidOperationException("Cinema not found.");

            Manager? manager = null;
            if (!string.IsNullOrWhiteSpace(model.ManagerEmail))
            {
                manager = await _managerRepository
                    .GetAllAttached()
                    .SingleOrDefaultAsync(m => m.User.Email == model.ManagerEmail);

                if (manager == null)
                    throw new InvalidOperationException("Selected manager does not exist.");
            }

         
            cinema.Name = model.Name;
            cinema.Location = model.Location;
            cinema.ManagerId = manager?.Id;

            await _cinemaRepository.UpdateAsync(cinema);
        }


        public async Task<bool> ToggleDeleteCinemaAsync(string cinemaId)
        {
            if (string.IsNullOrWhiteSpace(cinemaId))
                return false;

            var cinema = await _cinemaRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(c => c.Id.ToString() == cinemaId);

            if (cinema == null)
                return false;

            cinema.IsDeleted = !cinema.IsDeleted;
            await _cinemaRepository.UpdateAsync(cinema);

            return true; 
        }













    }

}
