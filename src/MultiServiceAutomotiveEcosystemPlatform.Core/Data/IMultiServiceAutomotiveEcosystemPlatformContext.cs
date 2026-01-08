using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Data;

public interface IMultiServiceAutomotiveEcosystemPlatformContext
{
    // Tenant Aggregate
    DbSet<Tenant> Tenants { get; }

    // Customer Aggregate
    DbSet<Customer> Customers { get; }
    DbSet<CustomerOwnershipHistory> CustomerOwnershipHistories { get; }

    // Professional Aggregate
    DbSet<Professional> Professionals { get; }
    DbSet<ProfessionalSpecialty> ProfessionalSpecialties { get; }
    DbSet<SpecialtyCatalog> SpecialtyCatalogs { get; }

    // Referral Aggregate
    DbSet<CustomerReferral> CustomerReferrals { get; }
    DbSet<ProfessionalReferral> ProfessionalReferrals { get; }
    DbSet<ReferralCode> ReferralCodes { get; }
    DbSet<ReferralStats> ReferralStats { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
