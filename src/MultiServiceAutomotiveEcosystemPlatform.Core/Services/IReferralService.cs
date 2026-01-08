using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public interface IReferralService
{
    // Customer Referral Operations
    Task<CustomerReferral> CreateCustomerReferralAsync(
        Guid referrerCustomerId,
        string? refereeEmail,
        string? refereePhone,
        string? refereeName,
        Guid? targetProfessionalId = null,
        string? targetServiceType = null,
        string? referralSource = null,
        CancellationToken cancellationToken = default);

    Task<CustomerReferral?> GetCustomerReferralByIdAsync(Guid referralId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerReferral>> GetCustomerReferralsByReferrerAsync(Guid referrerCustomerId, CancellationToken cancellationToken = default);
    Task<CustomerReferral?> FindPendingReferralByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<CustomerReferral?> FindPendingReferralByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    Task<CustomerReferral> ConvertCustomerReferralAsync(
        Guid referralId,
        Guid refereeCustomerId,
        decimal? rewardAmount = null,
        RewardType rewardType = RewardType.Cash,
        CancellationToken cancellationToken = default);

    Task ApproveRewardAsync(Guid referralId, CancellationToken cancellationToken = default);
    Task MarkRewardPaidAsync(Guid referralId, CancellationToken cancellationToken = default);
    Task CancelCustomerReferralAsync(Guid referralId, CancellationToken cancellationToken = default);
    Task<int> ExpireOldReferralsAsync(CancellationToken cancellationToken = default);

    // Professional Referral Operations
    Task<ProfessionalReferral> CreateProfessionalReferralAsync(
        Guid sourceProfessionalId,
        Guid targetProfessionalId,
        Guid customerId,
        string? reason = null,
        string? serviceNeeded = null,
        string? notes = null,
        ReferralPriority priority = ReferralPriority.Normal,
        DiscountType discountType = DiscountType.None,
        decimal? discountValue = null,
        CancellationToken cancellationToken = default);

    Task<ProfessionalReferral?> GetProfessionalReferralByIdAsync(Guid referralId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProfessionalReferral>> GetSentProfessionalReferralsAsync(Guid professionalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProfessionalReferral>> GetReceivedProfessionalReferralsAsync(Guid professionalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProfessionalReferral>> GetPendingProfessionalReferralsAsync(Guid professionalId, CancellationToken cancellationToken = default);

    Task AcceptProfessionalReferralAsync(Guid referralId, CancellationToken cancellationToken = default);
    Task DeclineProfessionalReferralAsync(Guid referralId, string? reason = null, CancellationToken cancellationToken = default);
    Task CompleteProfessionalReferralAsync(Guid referralId, CancellationToken cancellationToken = default);

    // Referral Code Operations
    Task<ReferralCode> CreateReferralCodeAsync(
        Guid customerId,
        string? namePrefix = null,
        decimal? rewardAmount = null,
        decimal? discountPercentage = null,
        CancellationToken cancellationToken = default);

    Task<ReferralCode?> GetReferralCodeByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<ReferralCode?> GetCustomerReferralCodeAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<bool> ValidateReferralCodeAsync(string code, CancellationToken cancellationToken = default);
    Task IncrementCodeUsageAsync(string code, CancellationToken cancellationToken = default);

    // Statistics
    Task<ReferralStats?> GetReferralStatsAsync(ReferralEntityType entityType, Guid entityId, CancellationToken cancellationToken = default);
    Task<ReferralStats> UpdateReferralStatsAsync(ReferralEntityType entityType, Guid entityId, CancellationToken cancellationToken = default);
}
