namespace CinemaApp.Web.ViewModels.Admin.UserManagment
{
    public class UserManagmentIndexViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;

        public IEnumerable<string> Roles { get; set; } = null!;
    }
}