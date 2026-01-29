using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IEnumerable<SelectListItem> Managers { get; set; } = new List<SelectListItem>();


        [Required(ErrorMessage = "Please select a manager")]
        [EmailAddress]
        public string? ManagerEmail { get; set; } 





    }
}
