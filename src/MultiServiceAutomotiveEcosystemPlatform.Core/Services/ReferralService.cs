using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public class ReferralService : IReferralService
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IReferralCodeGenerator _codeGenerator;

    public ReferralService(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext,
        IReferralCodeGenerator codeGenerator)
    {
        _context = context;
        _tenantContext = tenantContext;
        _codeGenerator = codeGenerator;
    }

    #region Customer Referral Operations

    public async Task<CustomerReferral> CreateCustomerReferralAsync(
        Guid referrerCustomerId,
        string? refereeEmail,
        string? refereePhone,
        string? refereeName,
        Guid? targetProfessionalId = null,
        string? targetServiceType = null,
        string? referralSource = null,
        CancellationToken cancellationToken = default)
    {
        // Get or create referrer's code
        var referrerCode = await GetCustomerReferralCodeAsync(referrerCustomerId, cancellationToken);
        if (referrerCode == null)
        {
            referrerCode = await CreateReferralCodeAsync(referrerCustomerId, cancellationToken: cancellationToken);
        }

        // Check for duplicate referral
        if (!string.IsNullOrWhiteSpace(refereeEmail))
        {
            var existing = await FindPendingReferralByEmailAsync(refereeEmail, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"A pending referral already exists for email '{refereeEmail}'.");
        }

        var referral = new CustomerReferral(
            _tenantContext.TenantId,
            referrerCustomerId,
            referrerCode.Code,
            refereeEmail,
            refereePhone,
            refereeName,
            targetProfessionalId,
            targetServiceType,
            referralSource);

        _context.CustomerReferrals.Add(referral);
        await _context.SaveChangesAsync(cancellationToken);

        // Update stats
        await UpdateReferralStatsAsync(ReferralEntityType.Customer, referrerCustomerId, cancellationToken);

        return referral;
    }

    public async Task<CustomerReferral?> GetCustomerReferralByIdAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals
            .FirstOrDefaultAsync(r => r.CustomerReferralId == referralId && r.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<IEnumerable<CustomerReferral>> GetCustomerReferralsByReferrerAsync(Guid referrerCustomerId, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals
            .Where(r => r.ReferrerCustomerId == referrerCustomerId && r.TenantId == _tenantContext.TenantId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerReferral?> FindPendingReferralByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();
        return await _context.CustomerReferrals
            .FirstOrDefaultAsync(r =>
                r.RefereeEmail == normalizedEmail &&
                (r.Status == CustomerReferralStatus.Pending || r.Status == CustomerReferralStatus.Contacted) &&
                r.TenantId == _tenantContext.TenantId,
                cancellationToken);
    }

    public async Task<CustomerReferral?> FindPendingReferralByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        var normalizedPhone = new string(phone.Where(char.IsDigit).ToArray());
        return await _context.CustomerReferrals
            .FirstOrDefaultAsync(r =>
                r.RefereePhone == normalizedPhone &&
                (r.Status == CustomerReferralStatus.Pending || r.Status == CustomerReferralStatus.Contacted) &&
                r.TenantId == _tenantContext.TenantId,
                cancellationToken);
    }

    public async Task<CustomerReferral> ConvertCustomerReferralAsync(
        Guid referralId,
        Guid refereeCustomerId,
        decimal? rewardAmount = null,
        RewardType rewardType = RewardType.Cash,
        CancellationToken cancellationToken = default)
    {
        var referral = await GetCustomerReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer referral with ID '{referralId}' not found.");

        referral.Convert(refereeCustomerId, rewardAmount, rewardType);
        await _context.SaveChangesAsync(cancellationToken);

        // Update stats
        await UpdateReferralStatsAsync(ReferralEntityType.Customer, referral.ReferrerCustomerId, cancellationToken);

        // Increment code usage
        await IncrementCodeUsageAsync(referral.ReferrerCode, cancellationToken);

        return referral;
    }

    public async Task ApproveRewardAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        var referral = await GetCustomerReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer referral with ID '{referralId}' not found.");

        referral.ApproveReward();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkRewardPaidAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        var referral = await GetCustomerReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer referral with ID '{referralId}' not found.");

        referral.MarkRewardPaid();
        await _context.SaveChangesAsync(cancellationToken);

        // Update stats
        await UpdateReferralStatsAsync(ReferralEntityType.Customer, referral.ReferrerCustomerId, cancellationToken);
    }

    public async Task CancelCustomerReferralAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        var referral = await GetCustomerReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer referral with ID '{referralId}' not found.");

        referral.Cancel();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> ExpireOldReferralsAsync(CancellationToken cancellationToken = default)
    {
        var expiredReferrals = await _context.CustomerReferrals
            .Where(r =>
                r.TenantId == _tenantContext.TenantId &&
                (r.Status == CustomerReferralStatus.Pending || r.Status == CustomerReferralStatus.Contacted) &&
                r.ExpiresAt != null &&
                r.ExpiresAt < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var referral in expiredReferrals)
        {
            referral.Expire();
        }

        if (expiredReferrals.Count > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return expiredReferrals.Count;
    }

    #endregion

    #region Professional Referral Operations

    public async Task<ProfessionalReferral> CreateProfessionalReferralAsync(
        Guid sourceProfessionalId,
        Guid targetProfessionalId,
        Guid customerId,
        string? reason = null,
        string? serviceNeeded = null,
        string? notes = null,
        ReferralPriority priority = ReferralPriority.Normal,
        DiscountType discountType = DiscountType.None,
        decimal? discountValue = null,
        CancellationToken cancellationToken = default)
    {
        var referral = new ProfessionalReferral(
            _tenantContext.TenantId,
            sourceProfessionalId,
            targetProfessionalId,
            customerId,
            reason,
            serviceNeeded,
            notes,
            priority);

        if (discountType != DiscountType.None)
        {
            var discountCode = _codeGenerator.GenerateDiscountCode();
            referral.SetDiscount(discountType, discountValue, discountCode);
        }

        _context.ProfessionalReferrals.Add(referral);
        await _context.SaveChangesAsync(cancellationToken);

        // Update stats for both professionals
        await UpdateReferralStatsAsync(ReferralEntityType.Professional, sourceProfessionalId, cancellationToken);
        await UpdateReferralStatsAsync(ReferralEntityType.Professional, targetProfessionalId, cancellationToken);

        return referral;
    }

    public async Task<ProfessionalReferral?> GetProfessionalReferralByIdAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .FirstOrDefaultAsync(r => r.ProfessionalReferralId == referralId && r.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<IEnumerable<ProfessionalReferral>> GetSentProfessionalReferralsAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .Where(r => r.SourceProfessionalId == professionalId && r.TenantId == _tenantContext.TenantId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProfessionalReferral>> GetReceivedProfessionalReferralsAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .Where(r => r.TargetProfessionalId == professionalId && r.TenantId == _tenantContext.TenantId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProfessionalReferral>> GetPendingProfessionalReferralsAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .Where(r =>
                r.TargetProfessionalId == professionalId &&
                r.Status == ProfessionalReferralStatus.Pending &&
                r.TenantId == _tenantContext.TenantId)
            .OrderByDescending(r => r.Priority)
            .ThenBy(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AcceptProfessionalReferralAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        var referral = await GetProfessionalReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional referral with ID '{referralId}' not found.");

        referral.Accept();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeclineProfessionalReferralAsync(Guid referralId, string? reason = null, CancellationToken cancellationToken = default)
    {
        var referral = await GetProfessionalReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional referral with ID '{referralId}' not found.");

        referral.Decline(reason);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteProfessionalReferralAsync(Guid referralId, CancellationToken cancellationToken = default)
    {
        var referral = await GetProfessionalReferralByIdAsync(referralId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional referral with ID '{referralId}' not found.");

        referral.Complete();
        await _context.SaveChangesAsync(cancellationToken);

        // Update stats
        await UpdateReferralStatsAsync(ReferralEntityType.Professional, referral.SourceProfessionalId, cancellationToken);
        await UpdateReferralStatsAsync(ReferralEntityType.Professional, referral.TargetProfessionalId, cancellationToken);
    }

    #endregion

    #region Referral Code Operations

    public async Task<ReferralCode> CreateReferralCodeAsync(
        Guid customerId,
        string? namePrefix = null,
        decimal? rewardAmount = null,
        decimal? discountPercentage = null,
        CancellationToken cancellationToken = default)
    {
        // Check if customer already has a code
        var existingCode = await GetCustomerReferralCodeAsync(customerId, cancellationToken);
        if (existingCode != null)
            return existingCode;

        // Generate unique code
        string code;
        do
        {
            code = _codeGenerator.GenerateCode(namePrefix);
        }
        while (await _context.ReferralCodes.AnyAsync(c => c.Code == code, cancellationToken));

        var referralCode = new ReferralCode(
            _tenantContext.TenantId,
            code,
            ReferralCodeType.Customer,
            customerId: customerId,
            rewardAmount: rewardAmount,
            discountPercentage: discountPercentage);

        _context.ReferralCodes.Add(referralCode);
        await _context.SaveChangesAsync(cancellationToken);

        return referralCode;
    }

    public async Task<ReferralCode?> GetReferralCodeByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.ToUpperInvariant().Trim();
        return await _context.ReferralCodes
            .FirstOrDefaultAsync(c => c.Code == normalizedCode && c.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<ReferralCode?> GetCustomerReferralCodeAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .FirstOrDefaultAsync(c =>
                c.CustomerId == customerId &&
                c.CodeType == ReferralCodeType.Customer &&
                c.TenantId == _tenantContext.TenantId,
                cancellationToken);
    }

    public async Task<bool> ValidateReferralCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var referralCode = await GetReferralCodeByCodeAsync(code, cancellationToken);
        return referralCode?.CanBeUsed ?? false;
    }

    public async Task IncrementCodeUsageAsync(string code, CancellationToken cancellationToken = default)
    {
        var referralCode = await GetReferralCodeByCodeAsync(code, cancellationToken);
        if (referralCode != null)
        {
            referralCode.IncrementUses();
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    #endregion

    #region Statistics

    public async Task<ReferralStats?> GetReferralStatsAsync(ReferralEntityType entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralStats
            .FirstOrDefaultAsync(s =>
                s.EntityType == entityType &&
                s.EntityId == entityId &&
                s.TenantId == _tenantContext.TenantId,
                cancellationToken);
    }

    public async Task<ReferralStats> UpdateReferralStatsAsync(ReferralEntityType entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        var stats = await GetReferralStatsAsync(entityType, entityId, cancellationToken);

        if (stats == null)
        {
            stats = new ReferralStats(_tenantContext.TenantId, entityType, entityId);
            _context.ReferralStats.Add(stats);
        }

        if (entityType == ReferralEntityType.Customer)
        {
            var referrals = await _context.CustomerReferrals
                .Where(r => r.ReferrerCustomerId == entityId && r.TenantId == _tenantContext.TenantId)
                .ToListAsync(cancellationToken);

            var totalSent = referrals.Count;
            var successful = referrals.Count(r => r.Status == CustomerReferralStatus.Converted);
            var pending = referrals.Count(r => r.Status == CustomerReferralStatus.Pending || r.Status == CustomerReferralStatus.Contacted);
            var totalEarned = referrals.Where(r => r.RewardStatus == RewardStatus.Paid).Sum(r => r.RewardAmount ?? 0);
            var rewardsPending = referrals.Where(r => r.RewardStatus == RewardStatus.Pending || r.RewardStatus == RewardStatus.Approved).Sum(r => r.RewardAmount ?? 0);

            stats.UpdateCustomerStats(totalSent, successful, pending, totalEarned, rewardsPending);
        }
        else
        {
            var received = await _context.ProfessionalReferrals
                .CountAsync(r => r.TargetProfessionalId == entityId && r.TenantId == _tenantContext.TenantId, cancellationToken);
            var given = await _context.ProfessionalReferrals
                .CountAsync(r => r.SourceProfessionalId == entityId && r.TenantId == _tenantContext.TenantId, cancellationToken);
            var completed = await _context.ProfessionalReferrals
                .CountAsync(r => r.TargetProfessionalId == entityId && r.Status == ProfessionalReferralStatus.Completed && r.TenantId == _tenantContext.TenantId, cancellationToken);

            var conversionRate = received > 0 ? (decimal)completed / received : 0;

            var avgDiscount = await _context.ProfessionalReferrals
                .Where(r => r.SourceProfessionalId == entityId && r.DiscountOffered && r.TenantId == _tenantContext.TenantId)
                .AverageAsync(r => r.DiscountValue ?? 0, cancellationToken);

            stats.UpdateProfessionalStats(received, given, conversionRate, avgDiscount);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return stats;
    }

    #endregion
}
