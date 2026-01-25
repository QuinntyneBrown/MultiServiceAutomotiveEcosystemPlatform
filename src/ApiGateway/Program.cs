using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add YARP Reverse Proxy with Service Discovery
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed", context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            Window = TimeSpan.FromSeconds(10),
            PermitLimit = 100,
            QueueLimit = 10,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        }));

    options.AddPolicy("sliding", context => RateLimitPartition.GetSlidingWindowLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new SlidingWindowRateLimiterOptions
        {
            Window = TimeSpan.FromSeconds(30),
            PermitLimit = 100,
            SegmentsPerWindow = 3,
            QueueLimit = 10
        }));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure middleware pipeline
app.UseCors("AllowUI");

app.UseRateLimiter();

app.MapReverseProxy();

app.MapDefaultEndpoints();

app.MapGet("/", () => Results.Ok(new
{
    Service = "API Gateway",
    Status = "Running",
    Timestamp = DateTime.UtcNow
}));

app.Run();
