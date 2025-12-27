

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Movie
{
    public class MovieFormModelCreate
    {

       public MovieFormModelCreate()
        {
            this.ReleaseDate = DateTime.Today;

        }


        public string Id { get; set; } 

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Genre is required.")]
        [MaxLength(50, ErrorMessage = "Genre cannot exceed 50 characters.")]
        [MinLength(2, ErrorMessage = "Genre must be at least 2 characters long.")]
        public string Genre { get; set; } = null!;

        [Required(ErrorMessage = "Release date is required.")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Director is required.")]
        [MaxLength(150, ErrorMessage = "Director cannot exceed 150 characters.")]
        [MinLength(2, ErrorMessage = "Director must be at least 2 characters long.")]
        public string Director { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be positive.")]
        public int Duration { get; set; } // Duration in minutes

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; } // The URL of movie poster

        [Url(ErrorMessage = "Please enter a valid trailer URL.")]
        
        public string? TrailerUrl { get; set; }



    }
}
