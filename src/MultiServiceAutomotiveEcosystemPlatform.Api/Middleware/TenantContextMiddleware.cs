// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Security.Claims;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Middleware;

public sealed class TenantContextMiddleware
{
    private const string TenantIdClaimType = "tenant_id";
    private const string TenantIdHeaderName = "X-Tenant-Id";

    private readonly RequestDelegate _next;

    public TenantContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        if (!tenantContext.HasTenant)
        {
            var tenantId = ResolveTenantId(context);
            if (tenantId.HasValue)
            {
                tenantContext.SetTenant(tenantId.Value);
            }
        }

        await _next(context);
    }

    private static Guid? ResolveTenantId(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var claimValue = context.User.FindFirstValue(TenantIdClaimType);
            if (Guid.TryParse(claimValue, out var tenantIdFromClaim))
            {
                return tenantIdFromClaim;
            }
        }

        if (context.Request.Headers.TryGetValue(TenantIdHeaderName, out var headerValues)
            && Guid.TryParse(headerValues.FirstOrDefault(), out var tenantIdFromHeader))
        {
            return tenantIdFromHeader;
        }

        return null;
    }
}
