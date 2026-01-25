using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Behaviors;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        params Type[] assemblyMarkerTypes)
    {
        services.AddScoped<ITenantContext, TenantContext>();

        var assemblies = assemblyMarkerTypes.Select(t => t.Assembly).ToArray();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }

    public static IServiceCollection AddSharedInfrastructure<TAssemblyMarker>(this IServiceCollection services)
    {
        return services.AddSharedInfrastructure(typeof(TAssemblyMarker));
    }
}
