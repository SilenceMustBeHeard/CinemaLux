using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Admin.CinemaManagement
{
    public class CinemaManagementEditViewModel : CinemaManagementCreateViewModel
    {
        [Required]
        public string Id { get; set; } = null!;



    }
}
