var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var redis = builder.AddRedis("redis");

// SQL Server with databases
var sqlServer = builder.AddSqlServer("sql")
    .WithDataVolume("sql-data");

var tenantDb = sqlServer.AddDatabase("tenant-db");
var customerDb = sqlServer.AddDatabase("customer-db");
var professionalDb = sqlServer.AddDatabase("professional-db");
var referralDb = sqlServer.AddDatabase("referral-db");

// Tenant Service
var tenantApi = builder.AddProject<Projects.Tenant_Api>("tenant-api")
    .WithReference(tenantDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WaitFor(tenantDb)
    .WaitFor(rabbitMq);

// Customer Service
var customerApi = builder.AddProject<Projects.Customer_Api>("customer-api")
    .WithReference(customerDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WaitFor(customerDb)
    .WaitFor(rabbitMq)
    .WaitFor(tenantApi);

// Professional Service
var professionalApi = builder.AddProject<Projects.Professional_Api>("professional-api")
    .WithReference(professionalDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WaitFor(professionalDb)
    .WaitFor(rabbitMq)
    .WaitFor(tenantApi);

// Referral Service
var referralApi = builder.AddProject<Projects.Referral_Api>("referral-api")
    .WithReference(referralDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WaitFor(referralDb)
    .WaitFor(rabbitMq)
    .WaitFor(customerApi)
    .WaitFor(professionalApi);

// API Gateway
var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithExternalHttpEndpoints()
    .WithReference(tenantApi)
    .WithReference(customerApi)
    .WithReference(professionalApi)
    .WithReference(referralApi);

// Angular Frontend
builder.AddNpmApp("angular-ui", "../Ui")
    .WithReference(apiGateway)
    .WithHttpEndpoint(env: "PORT", port: 4200)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
