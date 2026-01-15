using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Core.Implementations
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _managerRepository
                .GetAllAttached()
                .AnyAsync(m => m.Id == id);
        }

        public async Task<bool> ExistsByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            return await _managerRepository
                .GetAllAttached()
                .AnyAsync(m => m.UserId == userId);
        }

      

        public async Task<Guid?> GetIdByUserIdAsync(string? userId)
        {
            Guid? managerId = null;

            if(!string.IsNullOrWhiteSpace(userId))
           { var manager = await _managerRepository.FirstOrDefaultAsync(m => m.UserId == userId);

                if (manager != null)
                {
                    managerId = manager.Id;
                }
            }
            return managerId;
            
        }
    }
}
