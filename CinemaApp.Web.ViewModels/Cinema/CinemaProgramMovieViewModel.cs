using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Cinema
{
    public class CinemaProgramMovieViewModel
    {

        public string MovieId { get; set; } = null!;


        public string Title { get; set; } = null!;

        public string Director { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;


    }
}
