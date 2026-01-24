using Microsoft.AspNetCore.Http;

namespace CinemaApp.Web.Infrastructure.MiddleWare
{
    public class AdminRedirectionMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRedirectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            // Only care about /admin
            if (path == null || !path.StartsWith("/admin"))
            {
                await _next(context);
                return;
            }

            var user = context.User;

            // Must be logged in AND Admin
            if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Admin"))
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            await _next(context);
        }
    }
}
