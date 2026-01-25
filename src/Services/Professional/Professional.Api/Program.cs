using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Professional.Api.Consumers;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Services;
using Shared.Messaging.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add SQL Server DbContext
builder.AddSqlServerDbContext<ProfessionalDbContext>("professional-db");

// Add Redis caching
builder.AddRedisDistributedCache("redis");

// Add messaging with consumers
builder.AddMessaging(x =>
{
    x.AddConsumer<TenantCreatedConsumer>();
    x.AddConsumer<TenantUpdatedConsumer>();
});

// Add shared infrastructure (MediatR, Validation, etc.)
builder.Services.AddSharedInfrastructure<Program>();

// Add tenant context
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Add repositories
builder.Services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
builder.Services.AddScoped<ISpecialtyCatalogRepository, SpecialtyCatalogRepository>();

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
    c.SwaggerDoc("v1", new() { Title = "Professional Service API", Version = "v1" });
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
    var db = scope.ServiceProvider.GetRequiredService<ProfessionalDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();

public partial class Program { }
