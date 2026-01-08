// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

public static class ReferralExtensions
{
    public static CustomerReferralDto ToDto(this CustomerReferral referral)
    {
        return new CustomerReferralDto
        {
            CustomerReferralId = referral.CustomerReferralId,
            TenantId = referral.TenantId,
            ReferrerCustomerId = referral.ReferrerCustomerId,
            ReferrerCode = referral.ReferrerCode,
            RefereeCustomerId = referral.RefereeCustomerId,
            RefereeEmail = referral.RefereeEmail,
            RefereePhone = referral.RefereePhone,
            RefereeName = referral.RefereeName,
            TargetProfessionalId = referral.TargetProfessionalId,
            TargetServiceType = referral.TargetServiceType,
            Status = referral.Status.ToString(),
            ConvertedAt = referral.ConvertedAt,
            RewardStatus = referral.RewardStatus.ToString(),
            RewardAmount = referral.RewardAmount,
            RewardType = referral.RewardType.ToString(),
            RewardPaidAt = referral.RewardPaidAt,
            ReferralSource = referral.ReferralSource,
            CreatedAt = referral.CreatedAt,
            UpdatedAt = referral.UpdatedAt,
            ExpiresAt = referral.ExpiresAt
        };
    }

    public static ProfessionalReferralDto ToDto(this ProfessionalReferral referral)
    {
        return new ProfessionalReferralDto
        {
            ProfessionalReferralId = referral.ProfessionalReferralId,
            TenantId = referral.TenantId,
            SourceProfessionalId = referral.SourceProfessionalId,
            TargetProfessionalId = referral.TargetProfessionalId,
            CustomerId = referral.CustomerId,
            Reason = referral.Reason,
            ServiceNeeded = referral.ServiceNeeded,
            Notes = referral.Notes,
            Priority = referral.Priority.ToString(),
            Status = referral.Status.ToString(),
            AcceptedAt = referral.AcceptedAt,
            CompletedAt = referral.CompletedAt,
            DeclinedReason = referral.DeclinedReason,
            DiscountOffered = referral.DiscountOffered,
            DiscountType = referral.DiscountType.ToString(),
            DiscountValue = referral.DiscountValue,
            DiscountCode = referral.DiscountCode,
            DiscountUsed = referral.DiscountUsed,
            CreatedAt = referral.CreatedAt,
            UpdatedAt = referral.UpdatedAt,
            ExpiresAt = referral.ExpiresAt
        };
    }

    public static ReferralCodeDto ToDto(this ReferralCode referralCode)
    {
        return new ReferralCodeDto
        {
            ReferralCodeId = referralCode.ReferralCodeId,
            Code = referralCode.Code,
            CodeType = referralCode.CodeType.ToString(),
            CustomerId = referralCode.CustomerId,
            ProfessionalId = referralCode.ProfessionalId,
            MaxUses = referralCode.MaxUses,
            CurrentUses = referralCode.CurrentUses,
            RewardAmount = referralCode.RewardAmount,
            DiscountPercentage = referralCode.DiscountPercentage,
            IsActive = referralCode.IsActive,
            ExpiresAt = referralCode.ExpiresAt,
            CreatedAt = referralCode.CreatedAt
        };
    }
}
