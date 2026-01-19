using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    public class AppUserMovie
    {
        public Guid AppUserId { get; set; } 
        public virtual AppUser AppUser { get; set; } = null!;
        public Guid MovieId { get; set; } 
        public virtual Movie Movie { get; set; } = null!;
        public bool IsActive { get; set; } = true;   
        public bool IsLiked { get; set; }        
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;


    }
}
