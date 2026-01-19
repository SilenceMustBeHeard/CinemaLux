using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    public class AppUser : IdentityUser<Guid>
    {

        public Guid? ManagerId { get; set; }
        public virtual Manager? Manager { get; set; }

        public virtual ICollection<AppUserMovie> WatchList { get; set; }
            = new HashSet<AppUserMovie>();




        public virtual ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();





    }

}
