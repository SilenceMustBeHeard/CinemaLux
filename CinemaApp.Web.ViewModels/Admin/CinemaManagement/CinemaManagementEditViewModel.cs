using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Web.ViewModels.Admin.CinemaManagement
{
    public class CinemaManagementEditViewModel : CinemaManagementCreateViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
    }
}