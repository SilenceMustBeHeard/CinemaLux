using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }


        public Guid CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; } = null!;

        public decimal Price { get; set; }
        public Guid CinemaMovieId { get; set; }
        public virtual CinemaMovie CinemaMovieProjections { get; set; } = null!;


        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;



    }
}

