using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Messaging.Configuration;

namespace Shared.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IHostApplicationBuilder AddMessaging(
        this IHostApplicationBuilder builder,
        Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            configureConsumers?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("rabbitmq");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    cfg.Host(new Uri(connectionString));
                }
                else
                {
                    var rabbitMqSettings = builder.Configuration
                        .GetSection(RabbitMqSettings.SectionName)
                        .Get<RabbitMqSettings>() ?? new RabbitMqSettings();

                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                }

                cfg.ConfigureEndpoints(context);
            });
        });

        return builder;
    }

    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            configureConsumers?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                var connectionString = configuration.GetConnectionString("rabbitmq");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    cfg.Host(new Uri(connectionString));
                }
                else
                {
                    var rabbitMqSettings = configuration
                        .GetSection(RabbitMqSettings.SectionName)
                        .Get<RabbitMqSettings>() ?? new RabbitMqSettings();

                    cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, rabbitMqSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });
                }

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
