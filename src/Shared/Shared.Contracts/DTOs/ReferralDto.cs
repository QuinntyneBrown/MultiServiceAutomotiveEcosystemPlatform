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
    public Guid ReferredProfessionalId { get; init; }
    public string? ReferredProfessionalName { get; init; }
    public Guid CustomerId { get; init; }
    public string? CustomerName { get; init; }
    public ProfessionalReferralStatus Status { get; init; }
    public string? ServiceType { get; init; }
    public string? Notes { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public string? DiscountCode { get; init; }
    public bool DiscountUsed { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? AcceptedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime? DeclinedAt { get; init; }
}

public record ReferralCodeDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public string Code { get; init; } = string.Empty;
    public ReferralCodeType Type { get; init; }
    public Guid OwnerId { get; init; }
    public string? OwnerName { get; init; }
    public int MaxUses { get; init; }
    public int CurrentUses { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public bool IsActive { get; init; }
    public decimal? RewardAmount { get; init; }
    public string? RewardType { get; init; }
    public decimal? DiscountPercentage { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record ReferralStatsDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public Guid OwnerId { get; init; }
    public string OwnerType { get; init; } = string.Empty;
    public int TotalReferrals { get; init; }
    public int SuccessfulReferrals { get; init; }
    public int PendingReferrals { get; init; }
    public decimal TotalRewardsEarned { get; init; }
    public decimal TotalRewardsPaid { get; init; }
    public DateTime LastReferralAt { get; init; }
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
    public Guid ReferredProfessionalId { get; init; }
    public Guid CustomerId { get; init; }
    public string? ServiceType { get; init; }
    public string? Notes { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
}

public record CreateReferralCodeRequest
{
    public ReferralCodeType Type { get; init; }
    public Guid OwnerId { get; init; }
    public int MaxUses { get; init; } = 0; // 0 = unlimited
    public DateTime? ExpiresAt { get; init; }
    public decimal? RewardAmount { get; init; }
    public string? RewardType { get; init; }
    public decimal? DiscountPercentage { get; init; }
}

public record ConvertReferralRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Phone { get; init; }
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
