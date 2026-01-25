using Shared.Contracts.Enums;

namespace Shared.Contracts.DTOs;

public record CustomerReferralDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public Guid ReferringCustomerId { get; init; }
    public string? ReferringCustomerName { get; init; }
    public string ReferredEmail { get; init; } = string.Empty;
    public string? ReferredPhone { get; init; }
    public string? ReferredFirstName { get; init; }
    public string? ReferredLastName { get; init; }
    public Guid? ConvertedCustomerId { get; init; }
    public CustomerReferralStatus Status { get; init; }
    public string? ReferralSource { get; init; }
    public string? ReferralCode { get; init; }
    public string? Notes { get; init; }
    public RewardStatus RewardStatus { get; init; }
    public decimal? RewardAmount { get; init; }
    public string? RewardType { get; init; }
    public DateTime? RewardPaidAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ContactedAt { get; init; }
    public DateTime? ConvertedAt { get; init; }
}

public record ProfessionalReferralDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public Guid ReferringProfessionalId { get; init; }
    public string? ReferringProfessionalName { get; init; }
    public string ReferredBusinessName { get; init; } = string.Empty;
    public string ReferredEmail { get; init; } = string.Empty;
    public string? ReferredPhone { get; init; }
    public string? ContactPersonName { get; init; }
    public Guid? ConvertedProfessionalId { get; init; }
    public ProfessionalReferralStatus Status { get; init; }
    public string? ReferralSource { get; init; }
    public string? ReferralCode { get; init; }
    public string? Notes { get; init; }
    public RewardStatus RewardStatus { get; init; }
    public decimal? RewardAmount { get; init; }
    public string? RewardType { get; init; }
    public DateTime? RewardPaidAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ContactedAt { get; init; }
    public DateTime? ConvertedAt { get; init; }
}

public record ReferralCodeDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public string Code { get; init; } = string.Empty;
    public Guid? CustomerId { get; init; }
    public Guid? ProfessionalId { get; init; }
    public string? CampaignName { get; init; }
    public int? MaxUses { get; init; }
    public int CurrentUses { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record ReferralStatsDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public Guid? CustomerId { get; init; }
    public Guid? ProfessionalId { get; init; }
    public int TotalReferrals { get; init; }
    public int SuccessfulReferrals { get; init; }
    public int PendingReferrals { get; init; }
    public decimal TotalRewardsEarned { get; init; }
    public DateTime? LastReferralAt { get; init; }
    public DateTime? LastSuccessfulReferralAt { get; init; }
}

public record CreateCustomerReferralRequest
{
    public Guid ReferringCustomerId { get; init; }
    public string ReferredEmail { get; init; } = string.Empty;
    public string? ReferredPhone { get; init; }
    public string? ReferredFirstName { get; init; }
    public string? ReferredLastName { get; init; }
    public string? ReferralSource { get; init; }
    public string? ReferralCode { get; init; }
    public string? Notes { get; init; }
}

public record CreateProfessionalReferralRequest
{
    public Guid ReferringProfessionalId { get; init; }
    public string ReferredBusinessName { get; init; } = string.Empty;
    public string ReferredEmail { get; init; } = string.Empty;
    public string? ReferredPhone { get; init; }
    public string? ContactPersonName { get; init; }
    public string? ReferralSource { get; init; }
    public string? ReferralCode { get; init; }
    public string? Notes { get; init; }
}

public record GenerateReferralCodeRequest
{
    public Guid? CustomerId { get; init; }
    public Guid? ProfessionalId { get; init; }
    public string? CampaignName { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public int? MaxUses { get; init; }
}

public record ValidateReferralCodeRequest
{
    public string Code { get; init; } = string.Empty;
}

public record ValidateReferralCodeResponse
{
    public bool IsValid { get; init; }
    public string? Message { get; init; }
    public ReferralCodeDto? Code { get; init; }
}
