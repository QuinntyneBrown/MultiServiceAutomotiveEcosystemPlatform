using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Tests.Data;

public class DatabaseSeederTests
{
    private MultiServiceAutomotiveEcosystemPlatformContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MultiServiceAutomotiveEcosystemPlatformContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new MultiServiceAutomotiveEcosystemPlatformContext(options, tenantContext: null);
    }

    [Fact]
    public async Task SeedAsync_SeedsTenant_WhenNoTenantsExist()
    {
        // Arrange
        using var context = CreateContext();
        var seeder = new DatabaseSeeder(context);

        // Act
        await seeder.SeedAsync();

        // Assert
        var tenants = await context.Tenants.ToListAsync();
        Assert.Single(tenants);
        Assert.Equal("demo-network", tenants[0].Slug);
        Assert.Equal("Demo Automotive Network", tenants[0].Name);
    }

    [Fact]
    public async Task SeedAsync_SeedsSpecialtyCatalog_WhenNoSpecialtiesExist()
    {
        // Arrange
        using var context = CreateContext();
        var seeder = new DatabaseSeeder(context);

        // Act
        await seeder.SeedAsync();

        // Assert
        var specialties = await context.SpecialtyCatalogs.ToListAsync();
        Assert.NotEmpty(specialties);
        Assert.Contains(specialties, s => s.Name == "General Automotive Repair");
        Assert.Contains(specialties, s => s.Name == "German Vehicle Specialist");
        Assert.Contains(specialties, s => s.Name == "EV Charger Installation");
    }

    [Fact]
    public async Task SeedAsync_DoesNotSeedTenant_WhenTenantsAlreadyExist()
    {
        // Arrange
        using var context = CreateContext();
        var existingTenant = new Core.Models.TenantAggregate.Tenant(
            slug: "existing-tenant",
            name: "Existing Tenant",
            displayName: "Existing");
        context.Tenants.Add(existingTenant);
        await context.SaveChangesAsync();

        var seeder = new DatabaseSeeder(context);

        // Act
        await seeder.SeedAsync();

        // Assert
        var tenants = await context.Tenants.ToListAsync();
        Assert.Single(tenants);
        Assert.Equal("existing-tenant", tenants[0].Slug);
    }

    [Fact]
    public async Task SeedAsync_DoesNotSeedSpecialties_WhenSpecialtiesAlreadyExist()
    {
        // Arrange
        using var context = CreateContext();
        var existingSpecialty = new Core.Models.ProfessionalAggregate.SpecialtyCatalog(
            name: "Existing Specialty",
            category: "Test");
        context.SpecialtyCatalogs.Add(existingSpecialty);
        await context.SaveChangesAsync();

        var seeder = new DatabaseSeeder(context);

        // Act
        await seeder.SeedAsync();

        // Assert
        var specialties = await context.SpecialtyCatalogs.ToListAsync();
        Assert.Single(specialties);
        Assert.Equal("Existing Specialty", specialties[0].Name);
    }
}
