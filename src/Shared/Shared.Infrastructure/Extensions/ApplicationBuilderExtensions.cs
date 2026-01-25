using Microsoft.AspNetCore.Builder;
using Shared.Infrastructure.Middleware;

namespace Shared.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSharedInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<TenantContextMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
