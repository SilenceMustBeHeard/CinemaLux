namespace CinemaApp.Web.ViewModels.Admin.CinemaManagement
{
    public class CinemaManagementIndexViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string? ManagerName { get; set; }
    }
}