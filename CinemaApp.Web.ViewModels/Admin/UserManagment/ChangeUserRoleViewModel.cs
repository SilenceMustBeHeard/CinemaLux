namespace CinemaApp.Web.ViewModels.Admin.UserManagment
{
    public class ChangeUserRoleViewModel
    {
        public Guid UserId { get; set; }
        public string NewRole { get; set; } = string.Empty;
    }
}