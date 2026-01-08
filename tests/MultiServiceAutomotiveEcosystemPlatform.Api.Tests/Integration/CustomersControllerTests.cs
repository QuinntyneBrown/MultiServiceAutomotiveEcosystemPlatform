// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;
using Xunit;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Tests.Integration;

public class CustomersControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

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
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>(JsonOptions);
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
        var createdCustomer = await createResponse.Content.ReadFromJsonAsync<CustomerDto>(JsonOptions);

        // Act
        var response = await _client.GetAsync($"/api/customers/{createdCustomer!.CustomerId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>(JsonOptions);
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
                Phone = $"555000{i}0000",
                FirstName = $"Customer{i}",
                LastName = "Test"
            };
            await _client.PostAsJsonAsync("/api/customers", command);
        }

        // Act
        var response = await _client.GetAsync("/api/customers?page=1&pageSize=10");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<GetCustomersQueryResponse>(JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Customers.Count >= 3);
    }
}

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _inMemoryDbName;
    private Guid _defaultTenantId;

    public TestWebApplicationFactory()
    {
        _inMemoryDbName = $"msauto-tests-{Guid.NewGuid():N}";

        // Set via environment variables so the app sees this when building IConfiguration.
        Environment.SetEnvironmentVariable("Database__Provider", "InMemory");
        Environment.SetEnvironmentVariable("Database__InMemoryName", _inMemoryDbName);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["Database:Provider"] = "InMemory",
                ["Database:InMemoryName"] = _inMemoryDbName,
            };

            configBuilder.AddInMemoryCollection(settings);
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Create database and seed test data
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MultiServiceAutomotiveEcosystemPlatformContext>();
        
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Seed test data
        _defaultTenantId = SeedTestData(context);

        return host;
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        if (_defaultTenantId != Guid.Empty)
        {
            client.DefaultRequestHeaders.Remove("X-Tenant-Id");
            client.DefaultRequestHeaders.Add("X-Tenant-Id", _defaultTenantId.ToString());
        }
    }

    private static Guid SeedTestData(MultiServiceAutomotiveEcosystemPlatformContext context)
    {
        // Add a default tenant for testing
        var tenant = new MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate.Tenant(
            "test-tenant",
            "Test Tenant",
            "Test Tenant");

        context.Tenants.Add(tenant);
        context.SaveChanges();

        return tenant.TenantId;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // InMemory DB is process-local; no explicit cleanup required.
        }
        base.Dispose(disposing);
    }
}
