using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.WatchList
{
    public class WatchListViewModel
    {
        public string MovieId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string ReleaseDate { get; set; } = null!;

        public string? ImageUrl { get; set; } = null!;

        public string? TrailerUrl { get; set; } = null!;



    }
}
