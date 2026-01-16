using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IProjectionService
    {
        Task <IEnumerable<string>> GetAllProjectionShowTimesAsync(string? cinemaId, string? movieId);
    }
}
