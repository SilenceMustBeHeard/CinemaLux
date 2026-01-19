using CinemaApp.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static CinemaApp.GCommon.AppConstants;

namespace CinemaApp.Web.Infrastructure.MiddleWare
{
    public class ManagerAccessMiddleWare
    {
        private readonly RequestDelegate _next;

        public ManagerAccessMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IManagerService managerService)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            // Only protect /manager routes
            if (path == null || !path.StartsWith("/manager"))
            {
                await _next(context);
                return;
            }

            var user = context.User;

            // Must be authenticated + Manager role
            if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Manager"))
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            //  UserId MUST be GUID
            string? userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            // Get managerId from DB
            Guid? managerId = await managerService.GetIdByUserIdAsync(userId);

            if (managerId == null)
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            //  Cookie validation
            if (!context.Request.Cookies.TryGetValue(ManagerCookie, out string? cookieValue))
            {
                BuildManagerCookie(context, managerId.Value);
            }
            else
            {
                string expectedHash = HashManagerId(managerId.Value);

                if (!string.Equals(expectedHash, cookieValue, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }
            }

            await _next(context);
        }

        //  Helpers 

        private static void BuildManagerCookie(HttpContext context, Guid managerId)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromHours(4)
            };

            string hashedManagerId = HashManagerId(managerId);

            context.Response.Cookies.Append(ManagerCookie, hashedManagerId, cookieOptions);
        }

        private static string HashManagerId(Guid managerId)
        {
            using var sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(managerId.ToString());
            byte[] hash = sha512.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
