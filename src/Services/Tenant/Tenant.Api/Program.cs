using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Extensions;
using Shared.Messaging.Extensions;
using Tenant.Core.Interfaces;
using Tenant.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add SQL Server DbContext
builder.AddSqlServerDbContext<TenantDbContext>("tenant-db");

// Add Redis caching
builder.AddRedisDistributedCache("redis");

// Add messaging
builder.AddMessaging();

// Add shared infrastructure (MediatR, Validation, etc.)
builder.Services.AddSharedInfrastructure<Program>();

// Add repository
builder.Services.AddScoped<ITenantRepository, TenantRepository>();

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
    c.SwaggerDoc("v1", new() { Title = "Tenant Service API", Version = "v1" });
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
    var db = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();

public partial class Program { }
