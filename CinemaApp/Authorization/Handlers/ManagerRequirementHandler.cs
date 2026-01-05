using CinemaApp.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CinemaApp.Web.Authorization.Requirements;

namespace CinemaApp.Web.Authorization.Handlers
{
    public class ManagerRequirementHandler
        : AuthorizationHandler<ManagerRequirement>
    {
        private readonly IManagerService _managerService;

        public ManagerRequirementHandler(IManagerService managerService)
        {
            _managerService = managerService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ManagerRequirement requirement)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return;

            if (!context.User.IsInRole("Manager"))
                return;

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _managerService.ExistsByUserIdAsync(userId))
            {
                context.Succeed(requirement);
            }
        }
    }
}
