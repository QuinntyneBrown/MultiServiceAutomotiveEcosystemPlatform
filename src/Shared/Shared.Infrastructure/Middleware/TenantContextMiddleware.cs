using Microsoft.AspNetCore.Http;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Middleware;

public class TenantContextMiddleware
{
    private readonly RequestDelegate _next;
    private const string TenantIdClaimType = "tenant_id";
    private const string TenantIdHeader = "X-Tenant-Id";

    public TenantContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        var tenantId = GetTenantIdFromClaims(context) ?? GetTenantIdFromHeader(context);

        if (tenantId.HasValue)
        {
            tenantContext.SetTenant(tenantId.Value);
        }

        await _next(context);
    }

    private static Guid? GetTenantIdFromClaims(HttpContext context)
    {
        var tenantClaim = context.User?.Claims.FirstOrDefault(c => c.Type == TenantIdClaimType);
        if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
        {
            return tenantId;
        }
        return null;
    }

    private static Guid? GetTenantIdFromHeader(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(TenantIdHeader, out var tenantIdHeader))
        {
            if (Guid.TryParse(tenantIdHeader.FirstOrDefault(), out var tenantId))
            {
                return tenantId;
            }
        }
        return null;
    }
}
