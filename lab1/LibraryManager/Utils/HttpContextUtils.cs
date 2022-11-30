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
    }
}
