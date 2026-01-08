using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Tests.Data;

public class MultiServiceAutomotiveEcosystemPlatformContextTests
{
    private MultiServiceAutomotiveEcosystemPlatformContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MultiServiceAutomotiveEcosystemPlatformContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new MultiServiceAutomotiveEcosystemPlatformContext(options, tenantContext: null);
    }

    [Fact]
    public async Task CanAddAndRetrieveTenant()
    {
        // Arrange
        using var context = CreateContext();
        var tenant = new Tenant(
            slug: "test-tenant",
            name: "Test Tenant",
            displayName: "Test Tenant Display");

        // Act
        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();

        // Assert
        var retrievedTenant = await context.Tenants.FirstOrDefaultAsync(t => t.Slug == "test-tenant");
        Assert.NotNull(retrievedTenant);
        Assert.Equal("test-tenant", retrievedTenant.Slug);
        Assert.Equal("Test Tenant", retrievedTenant.Name);
    }

    [Fact]
    public async Task CanAddAndRetrieveCustomer()
    {
        // Arrange
        using var context = CreateContext();
        var tenantId = Guid.NewGuid();
        var customer = new Customer(
            tenantId: tenantId,
            email: "test@example.com",
            phone: "1234567890",
            firstName: "John",
            lastName: "Doe");

        // Act
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        // Assert
        var retrievedCustomer = await context.Customers.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Email == "test@example.com");
        Assert.NotNull(retrievedCustomer);
        Assert.Equal("John", retrievedCustomer.FirstName);
        Assert.Equal("Doe", retrievedCustomer.LastName);
        Assert.Equal(tenantId, retrievedCustomer.TenantId);
    }

    [Fact]
    public async Task CanAddAndRetrieveProfessional()
    {
        // Arrange
        using var context = CreateContext();
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var professional = new Professional(
            tenantId: tenantId,
            userId: userId,
            businessName: "Test Auto Shop",
            businessType: BusinessType.MechanicDomestic,
            firstName: "Jane",
            lastName: "Smith",
            email: "jane@testautoshop.com",
            phone: "9876543210",
            addressLine1: "123 Main St",
            city: "Toronto",
            province: "ON",
            postalCode: "M5H 2N2");

        // Act
        context.Professionals.Add(professional);
        await context.SaveChangesAsync();

        // Assert
        var retrievedProfessional = await context.Professionals.IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.BusinessName == "Test Auto Shop");
        Assert.NotNull(retrievedProfessional);
        Assert.Equal("Jane", retrievedProfessional.FirstName);
        Assert.Equal("Smith", retrievedProfessional.LastName);
        Assert.Equal(tenantId, retrievedProfessional.TenantId);
    }

    [Fact]
    public async Task CanAddAndRetrieveSpecialtyCatalog()
    {
        // Arrange
        using var context = CreateContext();
        var specialty = new SpecialtyCatalog(
            name: "Engine Repair",
            category: "Mechanical");

        // Act
        context.SpecialtyCatalogs.Add(specialty);
        await context.SaveChangesAsync();

        // Assert
        var retrievedSpecialty = await context.SpecialtyCatalogs
            .FirstOrDefaultAsync(s => s.Name == "Engine Repair");
        Assert.NotNull(retrievedSpecialty);
        Assert.Equal("engine-repair", retrievedSpecialty.Slug);
        Assert.Equal("Mechanical", retrievedSpecialty.Category);
    }

    [Fact]
    public async Task DbSetPropertiesAreNotNull()
    {
        // Arrange
        using var context = CreateContext();

        // Assert
        Assert.NotNull(context.Tenants);
        Assert.NotNull(context.Customers);
        Assert.NotNull(context.CustomerOwnershipHistories);
        Assert.NotNull(context.Professionals);
        Assert.NotNull(context.ProfessionalSpecialties);
        Assert.NotNull(context.SpecialtyCatalogs);
        Assert.NotNull(context.CustomerReferrals);
        Assert.NotNull(context.ProfessionalReferrals);
        Assert.NotNull(context.ReferralCodes);
        Assert.NotNull(context.ReferralStats);
    }
}
