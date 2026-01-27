using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Admin.CinemaManagement
{
    public class CinemaManagementCreateViewModel
    {
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;




        [MinLength(2)]
        [MaxLength(50)]
        public string Location { get; set; } = null!;
        public IEnumerable<string>? Managers { get; set; } 

        public string? ManagerEmail { get; set; } 





    }
}
