
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Cinema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemaRepository _cinemaRepo;

        public CinemaService(ICinemaRepository cinemaRepo) => _cinemaRepo= cinemaRepo;




        public async Task<IEnumerable<UsersCinemaIndexViewModel>> GetUserCinemasAsync()
       
                => await _cinemaRepo.GetAllAttached()
                .Select(c => new UsersCinemaIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location
                })
                .ToArrayAsync();


        
    }
}
