using System.Security.Claims;

namespace NotificationService.API.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var value = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(value is null) return Guid.NewGuid();
        return Guid.Parse(value);
    }
}
