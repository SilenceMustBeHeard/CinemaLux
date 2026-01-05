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
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && path.StartsWith("/manager"))
            {
                var user = context.User;

             // check if user is authenticated
                if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Manager"))
                {
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }

                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

              // check for cookie
                if (!context.Request.Cookies.TryGetValue(ManagerCookie, out var cookieValue))
                {
                    // take managerId from db
                    Guid? managerId = await managerService.GetIdByUserIdAsync(userId);

                    if (managerId == null)
                    {
                        context.Response.Redirect("/Home/AccessDenied");
                        return;
                    }

                    BuildManagerCookie(context, managerId.Value);
                }
                else
                {
                    // tale managerId from claims
                    string? managerIdClaim = user.FindFirstValue(ClaimTypes.Name);
                    if (managerIdClaim == null)
                    {
                        context.Response.Redirect("/Home/AccessDenied");
                        return;
                    }

                    string hashedManagerId = Sha512ManagerId(managerIdClaim);

                    // Compare hash from claim to db
                    if (!string.Equals(hashedManagerId, cookieValue, StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.Redirect("/Home/AccessDenied");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private void BuildManagerCookie(HttpContext context, Guid managerId)
        {
            var cookieBuilder = new CookieBuilder
            {
                Name = ManagerCookie,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                SecurePolicy = CookieSecurePolicy.SameAsRequest,
                MaxAge = TimeSpan.FromHours(4)
            };

            var options = cookieBuilder.Build(context);
            string hashed = Sha512ManagerId(managerId.ToString());

            context.Response.Cookies.Append(ManagerCookie, hashed, options);
        }

        private string Sha512ManagerId(string managerId)
        {
            using var sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(managerId);
            byte[] hash = sha512.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLower();
        }
    }
}
