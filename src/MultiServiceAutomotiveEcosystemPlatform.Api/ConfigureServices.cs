// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;
using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;
using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(isOriginAllowed: _ => true)
            .AllowCredentials()));

        services.AddDbContext<MultiServiceAutomotiveEcosystemPlatformContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("MultiServiceAutomotiveEcosystemPlatform.Infrastructure")));

        services.AddScoped<IMultiServiceAutomotiveEcosystemPlatformContext>(provider =>
            provider.GetRequiredService<MultiServiceAutomotiveEcosystemPlatformContext>());

        services.AddScoped<ITenantContext, TenantContext>();

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));

        // Register Services
        services.AddScoped<IReferralCodeGenerator, ReferralCodeGenerator>();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}