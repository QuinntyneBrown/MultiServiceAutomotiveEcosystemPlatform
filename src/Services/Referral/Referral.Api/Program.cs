using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Referral.Api.Consumers;
using Referral.Api.Sagas.ReferralConversion;
using Referral.Api.Sagas.RewardPayment;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Referral.Infrastructure.Repositories;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Services;
using Shared.Messaging.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add SQL Server DbContext
builder.AddSqlServerDbContext<ReferralDbContext>("referral-db");

// Add Redis caching
builder.AddRedisDistributedCache("redis");

// Add messaging with consumers and sagas
builder.AddMessaging(x =>
{
    x.AddConsumer<CustomerCreatedConsumer>();
    x.AddConsumer<ProfessionalCreatedConsumer>();

    // Add sagas with in-memory repository (for development)
    // In production, use a persistent repository (e.g., Entity Framework, Redis)
    x.AddSagaStateMachine<ReferralConversionSaga, ReferralConversionSagaState>()
        .InMemoryRepository();

    x.AddSagaStateMachine<RewardPaymentSaga, RewardPaymentSagaState>()
        .InMemoryRepository();
});

// Add shared infrastructure (MediatR, Validation, etc.)
builder.Services.AddSharedInfrastructure<Program>();

// Add tenant context
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Add repositories
builder.Services.AddScoped<ICustomerReferralRepository, CustomerReferralRepository>();
builder.Services.AddScoped<IProfessionalReferralRepository, ProfessionalReferralRepository>();
builder.Services.AddScoped<IReferralCodeRepository, ReferralCodeRepository>();
builder.Services.AddScoped<IReferralStatsRepository, ReferralStatsRepository>();

// Add controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Referral Service API", Version = "v1" });
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSharedInfrastructure();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();

// Auto-migrate database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ReferralDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();

public partial class Program { }
