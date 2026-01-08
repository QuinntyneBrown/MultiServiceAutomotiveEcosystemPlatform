// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;
using Xunit;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Tests.Integration;

public class CustomersControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public CustomersControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateCustomer_ReturnsCreatedCustomer()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Email = "test@example.com",
            Phone = "1234567890",
            FirstName = "John",
            LastName = "Doe",
            Source = "test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/customers", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(customer);
        Assert.Equal("John", customer.FirstName);
        Assert.Equal("Doe", customer.LastName);
        Assert.Equal("test@example.com", customer.Email);
    }

    [Fact]
    public async Task GetCustomer_ReturnsCustomer()
    {
        // Arrange - create a customer first
        var command = new CreateCustomerCommand
        {
            Email = "gettest@example.com",
            Phone = "9876543210",
            FirstName = "Jane",
            LastName = "Smith"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/customers", command);
        var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerDto>();

        // Act
        var response = await _client.GetAsync($"/api/customers/{createdCustomer!.CustomerId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(customer);
        Assert.Equal("Jane", customer.FirstName);
        Assert.Equal("Smith", customer.LastName);
    }

    [Fact]
    public async Task GetCustomers_ReturnsPaginatedList()
    {
        // Arrange - create some customers
        for (int i = 0; i < 3; i++)
        {
            var command = new CreateCustomerCommand
            {
                Email = $"list{i}@example.com",
                Phone = $"55500{i}",
                FirstName = $"Customer{i}",
                LastName = "Test"
            };
            await _client.PostAsJsonAsync("/api/customers", command);
        }

        // Act
        var response = await _client.GetAsync("/api/customers?page=1&pageSize=10");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<GetCustomersQueryResponse>();
        Assert.NotNull(result);
        Assert.True(result.Customers.Count >= 3);
    }
}

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public TestWebApplicationFactory()
    {
        // Use SQL Server LocalDB or test instance
        _databaseName = $"MultiServiceAutomotiveEcosystemPlatformTest_{Guid.NewGuid():N}";
        _connectionString = $"Server=(localdb)\\mssqllocaldb;Database={_databaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real database context registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<MultiServiceAutomotiveEcosystemPlatformContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add SQL Server database for testing
            services.AddDbContext<MultiServiceAutomotiveEcosystemPlatformContext>(options =>
            {
                options.UseSqlServer(_connectionString, 
                    b => b.MigrationsAssembly("MultiServiceAutomotiveEcosystemPlatform.Infrastructure"));
            });
        });

        var host = base.CreateHost(builder);

        // Create database and seed test data
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MultiServiceAutomotiveEcosystemPlatformContext>();
        
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Seed test data
        SeedTestData(context);

        return host;
    }

    private static void SeedTestData(MultiServiceAutomotiveEcosystemPlatformContext context)
    {
        // Add a default tenant for testing
        var tenant = new MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate.Tenant(
            "test-tenant",
            "Test Tenant",
            "Test Tenant");

        context.Tenants.Add(tenant);
        context.SaveChanges();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Clean up test database
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MultiServiceAutomotiveEcosystemPlatformContext>();
            context.Database.EnsureDeleted();
        }
        base.Dispose(disposing);
    }
}
