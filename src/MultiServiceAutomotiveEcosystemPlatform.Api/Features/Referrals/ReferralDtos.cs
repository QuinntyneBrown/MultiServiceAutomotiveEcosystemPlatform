// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

public class CustomerReferralDto
{
    public Guid CustomerReferralId { get; set; }
    public Guid TenantId { get; set; }
    public Guid ReferrerCustomerId { get; set; }
    public string ReferrerCode { get; set; } = string.Empty;
    public Guid? RefereeCustomerId { get; set; }
    public string? RefereeEmail { get; set; }
    public string? RefereePhone { get; set; }
    public string? RefereeName { get; set; }
    public Guid? TargetProfessionalId { get; set; }
    public string? TargetServiceType { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ConvertedAt { get; set; }
    public string RewardStatus { get; set; } = string.Empty;
    public decimal? RewardAmount { get; set; }
    public string RewardType { get; set; } = string.Empty;
    public DateTime? RewardPaidAt { get; set; }
    public string? ReferralSource { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class ProfessionalReferralDto
{
    public Guid ProfessionalReferralId { get; set; }
    public Guid TenantId { get; set; }
    public Guid SourceProfessionalId { get; set; }
    public Guid TargetProfessionalId { get; set; }
    public Guid CustomerId { get; set; }
    public string? Reason { get; set; }
    public string? ServiceNeeded { get; set; }
    public string? Notes { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? AcceptedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? DeclinedReason { get; set; }
    public bool DiscountOffered { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public decimal? DiscountValue { get; set; }
    public string? DiscountCode { get; set; }
    public bool DiscountUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class ReferralCodeDto
{
    public Guid ReferralCodeId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CodeType { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public Guid? ProfessionalId { get; set; }
    public int? MaxUses { get; set; }
    public int CurrentUses { get; set; }
    public decimal? RewardAmount { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
