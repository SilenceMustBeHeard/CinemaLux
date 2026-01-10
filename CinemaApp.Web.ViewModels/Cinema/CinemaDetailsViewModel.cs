using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Cinema
{
    public class CinemaDetailsViewModel
    {


        public string Name { get; set; }

        public string Location { get; set; }
        public IEnumerable<CinemaDetailsMovieViewModel> Movies { get; set; }
        = null!;
    }
}
