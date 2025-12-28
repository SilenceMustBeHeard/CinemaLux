using CinemaApp.Data;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.WatchList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core
{
    public class WatchListService: IWatchListService
    {
    private readonly CinemaAppDbContext _context;
        public WatchListService(CinemaAppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<WatchListViewModel>> GetWatchListByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
