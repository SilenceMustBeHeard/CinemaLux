using CinemaApp.Web.ViewModels.WatchList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IWatchListService
    {
        Task<IEnumerable<WatchListViewModel>> GetWatchListByUserIdAsync(string userId);
    }
}
