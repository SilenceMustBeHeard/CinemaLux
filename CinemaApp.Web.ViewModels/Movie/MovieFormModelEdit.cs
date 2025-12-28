using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Movie
{
    public class MovieFormModelEdit
    {
        public string Id { get; set; } = null!; 

        [Required, MaxLength(200), MinLength(2)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(50), MinLength(2)]
        public string Genre { get; set; } = null!;

        [Required, DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required, MaxLength(150), MinLength(2)]
        public string Director { get; set; } = null!;

        [Required, Range(1, int.MaxValue)]
        public int Duration { get; set; }

        [Required, MaxLength(500), MinLength(10)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
        [Url]
        public string? TrailerUrl { get; set; }
    }

}
