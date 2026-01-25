using Shared.Contracts.Enums;

namespace Shared.Contracts.DTOs;

public record CustomerDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? MiddleName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public PreferredContactMethod PreferredContactMethod { get; init; }
    public bool MarketingOptIn { get; init; }
    public bool SmsOptIn { get; init; }
    public CustomerStatus Status { get; init; }
    public bool EmailVerified { get; init; }
    public bool PhoneVerified { get; init; }
    public Guid? OwnerProfessionalId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreateCustomerRequest
{
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? MiddleName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public PreferredContactMethod PreferredContactMethod { get; init; } = PreferredContactMethod.Email;
    public bool MarketingOptIn { get; init; }
    public bool SmsOptIn { get; init; }
}

public record UpdateCustomerRequest
{
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? MiddleName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public PreferredContactMethod PreferredContactMethod { get; init; }
    public bool MarketingOptIn { get; init; }
    public bool SmsOptIn { get; init; }
}

public record AssignOwnerRequest
{
    public Guid ProfessionalId { get; init; }
    public string? Reason { get; init; }
}

public record TransferOwnershipRequest
{
    public Guid NewOwnerId { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public record CustomerOwnershipHistoryDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public Guid? PreviousOwnerId { get; init; }
    public Guid NewOwnerId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime TransferredAt { get; init; }
}

public record UpdateCustomerStatusRequest
{
    public CustomerStatus Status { get; init; }
}
