using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;

public class DatabaseSeeder
{
    private readonly MultiServiceAutomotiveEcosystemPlatformContext _context;

    public DatabaseSeeder(MultiServiceAutomotiveEcosystemPlatformContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedTenantsAsync();
        await SeedSpecialtyCatalogAsync();
        await _context.SaveChangesAsync();
    }

    private async Task SeedTenantsAsync()
    {
        if (await _context.Tenants.AnyAsync())
            return;

        var tenant = new Tenant(
            slug: "demo-network",
            name: "Demo Automotive Network",
            displayName: "Demo Network",
            logoUrl: null,
            primaryColor: "#1976d2",
            configuration: "{\"features\":{\"referrals\":true,\"loyalty\":true}}");

        _context.Tenants.Add(tenant);
    }

    private async Task SeedSpecialtyCatalogAsync()
    {
        if (await _context.SpecialtyCatalogs.AnyAsync())
            return;

        var specialties = new[]
        {
            // Mechanical Services
            new SpecialtyCatalog("General Automotive Repair", "Mechanical", null, "Complete automotive repair and maintenance services", "wrench"),
            new SpecialtyCatalog("Domestic Vehicle Specialist", "Mechanical", null, "Expert in American-made vehicles (Ford, GM, Chrysler)", "flag-usa"),
            new SpecialtyCatalog("German Vehicle Specialist", "Mechanical", null, "Specialized in BMW, Mercedes-Benz, Audi, VW, Porsche", "car-side"),
            new SpecialtyCatalog("Asian Vehicle Specialist", "Mechanical", null, "Expert in Honda, Toyota, Nissan, Mazda, Subaru", "car"),
            new SpecialtyCatalog("European Vehicle Specialist", "Mechanical", null, "Specialized in European imports", "car-front"),
            new SpecialtyCatalog("Engine Diagnostics", "Mechanical", null, "Advanced engine diagnostic services", "engine"),
            new SpecialtyCatalog("Transmission Repair", "Mechanical", null, "Automatic and manual transmission services", "gears"),
            new SpecialtyCatalog("Brake Service", "Mechanical", null, "Complete brake system repair and maintenance", "brake"),
            new SpecialtyCatalog("Electrical Systems", "Mechanical", null, "Automotive electrical diagnosis and repair", "bolt"),
            new SpecialtyCatalog("Air Conditioning Service", "Mechanical", null, "AC system repair and recharge", "snowflake"),

            // Body & Paint
            new SpecialtyCatalog("Collision Repair", "Body & Paint", null, "Complete collision and accident repair", "car-crash"),
            new SpecialtyCatalog("Auto Body Restoration", "Body & Paint", null, "Body panel repair and replacement", "paint-brush"),
            new SpecialtyCatalog("Custom Paint", "Body & Paint", null, "Custom painting and finishes", "palette"),
            new SpecialtyCatalog("Dent Removal", "Body & Paint", null, "Paintless dent removal and conventional repair", "hammer"),
            new SpecialtyCatalog("Frame Straightening", "Body & Paint", null, "Unibody and frame straightening", "straighten"),

            // Sales & Finance
            new SpecialtyCatalog("Used Car Sales", "Sales", null, "Pre-owned vehicle sales", "handshake"),
            new SpecialtyCatalog("New Car Sales", "Sales", null, "New vehicle sales and leasing", "car-showroom"),
            new SpecialtyCatalog("Auto Financing", "Finance", null, "Vehicle loans and financing solutions", "dollar-sign"),
            new SpecialtyCatalog("Car Buying Service", "Sales", null, "Professional car buying and acquisition", "cart-shopping"),
            new SpecialtyCatalog("Vehicle Appraisal", "Sales", null, "Professional vehicle valuation", "clipboard-check"),

            // Specialized Services
            new SpecialtyCatalog("EV Charger Installation", "Electrical", null, "Home and commercial EV charging station installation", "charging-station"),
            new SpecialtyCatalog("Electric Vehicle Service", "Mechanical", null, "Electric and hybrid vehicle maintenance", "leaf"),
            new SpecialtyCatalog("Performance Tuning", "Performance", null, "Performance upgrades and modifications", "gauge"),
            new SpecialtyCatalog("Custom Fabrication", "Customization", null, "Custom metal fabrication and modifications", "wrench-adjustable"),
            new SpecialtyCatalog("Detailing Services", "Detailing", null, "Professional automotive detailing", "sparkles"),

            // Tires & Wheels
            new SpecialtyCatalog("Tire Sales & Service", "Tires", null, "Tire sales, installation, and repair", "tire"),
            new SpecialtyCatalog("Wheel Alignment", "Tires", null, "Precision wheel alignment services", "compass"),
            new SpecialtyCatalog("Wheel & Rim Repair", "Tires", null, "Alloy wheel repair and refinishing", "circle"),

            // Maintenance
            new SpecialtyCatalog("Oil Change & Lubrication", "Maintenance", null, "Routine oil changes and fluid services", "oil-can"),
            new SpecialtyCatalog("Preventive Maintenance", "Maintenance", null, "Scheduled maintenance services", "calendar-check"),
            new SpecialtyCatalog("Provincial Inspections", "Inspection", null, "Vehicle safety and emissions inspections", "clipboard-list"),
        };

        _context.SpecialtyCatalogs.AddRange(specialties);
    }
}
