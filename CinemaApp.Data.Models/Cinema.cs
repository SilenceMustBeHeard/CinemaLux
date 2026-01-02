using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    public class Cinema
    {
        public Guid Id { get; set; }

        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        [MinLength(2)]
        [MaxLength(50)]
        public string Location { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<CinemaMovie> CinemaMovies { get; set; }
            = new HashSet<CinemaMovie>();

        public virtual ICollection<Ticket> Tickets { get; set; }
      = new HashSet<Ticket>();

    }
}
