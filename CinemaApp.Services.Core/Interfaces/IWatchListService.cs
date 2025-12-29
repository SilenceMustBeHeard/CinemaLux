using CinemaApp.Web.ViewModels.WatchList;
using Microsoft.AspNetCore.Mvc;
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

        Task ToggleWatchListAsync(string userId, string movieId);

        Task<bool> IsMovieInWatchListAsync(string userId, string movieId);
    }
}