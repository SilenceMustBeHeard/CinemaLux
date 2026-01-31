using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Admin.MovieManagement
{
    public class MovieManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;

        public int Duration { get; set; }
        public string Director { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
