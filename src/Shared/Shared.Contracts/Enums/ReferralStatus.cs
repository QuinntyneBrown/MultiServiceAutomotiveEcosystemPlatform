namespace Shared.Contracts.Enums;

public enum CustomerReferralStatus
{
    Pending = 0,
    Contacted = 1,
    Converted = 2,
    Expired = 3,
    Cancelled = 4
}

public enum ProfessionalReferralStatus
{
    Pending = 0,
    Accepted = 1,
    Declined = 2,
    Completed = 3,
    Expired = 4
}

public enum RewardStatus
{
    Pending = 0,
    Approved = 1,
    Paid = 2,
    Cancelled = 3
}

public enum ReferralCodeType
{
    Customer = 0,
    Professional = 1,
    Campaign = 2
}
