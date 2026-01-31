using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Admin.UserManagment
{
    public class ChangeUserRoleViewModel
    {
        public Guid UserId { get; set; }
        public string NewRole { get; set; } = string.Empty;
    }
}
