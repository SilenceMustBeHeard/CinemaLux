using CinemaApp.Web.Infrastructure.MiddleWare;
using Microsoft.AspNetCore.Builder;

namespace CinemaApp.Web.Infrastructure.Extensions
{
    public static class WebAppExtensions
    {
        public static IApplicationBuilder UseManagerAccessRestriction(this IApplicationBuilder app)
        {
            app.UseMiddleware<ManagerAccessMiddleWare>();
            return app;
        }
    }
}