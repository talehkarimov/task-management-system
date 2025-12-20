using System.Security.Claims;
using TaskService.API.Middlewares;
using TaskService.Application.Interfaces;

namespace TaskService.API.Context;

public sealed class HttpRequestContext(IHttpContextAccessor accessor) : IRequestContext
{
    public string? CorrelationId => accessor.HttpContext?.Items[CorrelationIdMiddleware.HeaderName]?.ToString();

    public Guid? UserId
    {
        get 
        {
            var raw = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out var guid) ? guid : null;
        }
    }

    public Guid? OrganizationId 
    {
        get
        {
            var raw = accessor.HttpContext?.Request.Headers["X-Org-Id"].ToString();
            return Guid.TryParse(raw, out var guid) ? guid : null;
        }

    }
}
