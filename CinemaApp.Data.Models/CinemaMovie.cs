using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    public class CinemaMovie
    {
        public Guid Id { get; set; }

        public Guid MovieId {  get; set; }
        public virtual Movie Movie { get; set; } = null!;


        public Guid CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; } = null!;
  
     public int AvailableTickets {  get; set; }
        public bool IsDeleted { get; set; } = false;


        // represent indicationg showtime 
        public int ShowTimes { get; set; }



        public virtual ICollection<Ticket> Tickets { get; set; }
        = new HashSet<Ticket>();    








    }
}
