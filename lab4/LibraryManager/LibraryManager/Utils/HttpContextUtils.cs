using LibraryManager.Core;
using System.Security.Claims;

namespace LibraryManager.Utils
{
    public static class HttpContextUtils
    {
        public static string? GetCurrentUsername(this HttpContext context)
        {
            var id = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                var username = context.User.FindFirst(ClaimTypes.Name);
                return username?.Value;
            }
            return null;
        }

        public static bool GetCurrentUserIsAdmin(this HttpContext context)
        {
            var id = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                var role = context.User.FindFirst(ClaimTypes.Role);
                return role?.Value == Roles.Admin;
            }
            return false;
        }
    }
}
