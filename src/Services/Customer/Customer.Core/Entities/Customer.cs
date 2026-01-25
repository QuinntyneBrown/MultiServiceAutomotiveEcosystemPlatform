using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Customer.Core.Entities;

public class Customer : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }

    // Contact Information
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }

    // Personal Information
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? MiddleName { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    // Address
    public string? StreetAddress { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Country { get; private set; }

    // Preferences
    public PreferredContactMethod PreferredContactMethod { get; private set; }
    public bool MarketingOptIn { get; private set; }
    public bool SmsOptIn { get; private set; }

    // Status
    public CustomerStatus Status { get; private set; }
    public bool EmailVerified { get; private set; }
    public bool PhoneVerified { get; private set; }

    // Ownership
    public Guid? OwnerProfessionalId { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation
    private readonly List<CustomerOwnershipHistory> _ownershipHistory = [];
    public IReadOnlyCollection<CustomerOwnershipHistory> OwnershipHistory => _ownershipHistory.AsReadOnly();

    private Customer() { }

    public Customer(
        Guid tenantId,
        string email,
        string? phone,
        string firstName,
        string lastName)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        Email = email.Trim().ToLowerInvariant();
        Phone = phone?.Trim();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Status = CustomerStatus.Active;
        PreferredContactMethod = PreferredContactMethod.Email;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string email, string? phone, PreferredContactMethod preferredContactMethod)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        Email = email.Trim().ToLowerInvariant();
        Phone = phone?.Trim();
        PreferredContactMethod = preferredContactMethod;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string? middleName, DateTime? dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        MiddleName = middleName?.Trim();
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(string? streetAddress, string? city, string? state, string? postalCode, string? country)
    {
        StreetAddress = streetAddress?.Trim();
        City = city?.Trim();
        State = state?.Trim();
        PostalCode = postalCode?.Trim();
        Country = country?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePreferences(bool marketingOptIn, bool smsOptIn)
    {
        MarketingOptIn = marketingOptIn;
        SmsOptIn = smsOptIn;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignOwner(Guid professionalId, string? reason = null)
    {
        var history = new CustomerOwnershipHistory(Id, OwnerProfessionalId, professionalId, reason ?? "Initial assignment");
        _ownershipHistory.Add(history);
        OwnerProfessionalId = professionalId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TransferOwnership(Guid newOwnerId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required for ownership transfer", nameof(reason));

        var history = new CustomerOwnershipHistory(Id, OwnerProfessionalId, newOwnerId, reason);
        _ownershipHistory.Add(history);
        OwnerProfessionalId = newOwnerId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void VerifyEmail()
    {
        EmailVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void VerifyPhone()
    {
        PhoneVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = CustomerStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = CustomerStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Block()
    {
        Status = CustomerStatus.Blocked;
        UpdatedAt = DateTime.UtcNow;
    }
}
