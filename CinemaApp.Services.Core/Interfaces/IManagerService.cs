using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CinemaApp.Services.Core.Interfaces
{
    public interface IManagerService
    {
        Task<Guid?> GetIdByUserIdAsync(Guid? userId);
        Task<bool> ExistsByIdAsync(Guid id);
        Task<bool> ExistsByUserIdAsync(Guid userId);
    }
}
