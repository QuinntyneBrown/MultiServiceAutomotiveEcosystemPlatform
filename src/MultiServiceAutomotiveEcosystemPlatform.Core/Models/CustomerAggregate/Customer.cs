using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;

public class Customer
{
    public Guid CustomerId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? OwnerProfessionalId { get; private set; }

    // Contact Information
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string? PhoneSecondary { get; private set; }

    // Personal Information
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime? DateOfBirth { get; private set; }

    // Address
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? City { get; private set; }
    public string? Province { get; private set; }
    public string? PostalCode { get; private set; }
    public string Country { get; private set; } = "CA";

    // Preferences
    public PreferredContactMethod PreferredContactMethod { get; private set; }
    public bool MarketingConsent { get; private set; }
    public bool NewsletterSubscribed { get; private set; }

    // Status
    public CustomerStatus Status { get; private set; }
    public bool EmailVerified { get; private set; }
    public bool PhoneVerified { get; private set; }

    // Metadata
    public string? Source { get; private set; }
    public string? Notes { get; private set; }
    public List<string> Tags { get; private set; } = new();

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? LastActivityAt { get; private set; }

    private Customer() { }

    public Customer(
        Guid tenantId,
        string email,
        string phone,
        string firstName,
        string lastName,
        Guid? ownerProfessionalId = null,
        string? source = null)
    {
        ValidateEmail(email);
        ValidatePhone(phone);
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        CustomerId = Guid.NewGuid();
        TenantId = tenantId;
        OwnerProfessionalId = ownerProfessionalId;
        Email = email.ToLowerInvariant().Trim();
        Phone = NormalizePhone(phone);
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Source = source;
        Status = CustomerStatus.Active;
        PreferredContactMethod = PreferredContactMethod.Email;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string email, string phone, string? phoneSecondary = null)
    {
        ValidateEmail(email);
        ValidatePhone(phone);

        Email = email.ToLowerInvariant().Trim();
        Phone = NormalizePhone(phone);
        PhoneSecondary = phoneSecondary != null ? NormalizePhone(phoneSecondary) : null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, DateTime? dateOfBirth = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(
        string? addressLine1,
        string? addressLine2,
        string? city,
        string? province,
        string? postalCode,
        string? country = null)
    {
        AddressLine1 = addressLine1?.Trim();
        AddressLine2 = addressLine2?.Trim();
        City = city?.Trim();
        Province = province?.Trim();
        PostalCode = postalCode?.Trim();
        if (!string.IsNullOrWhiteSpace(country))
            Country = country.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePreferences(
        PreferredContactMethod preferredContactMethod,
        bool marketingConsent,
        bool newsletterSubscribed)
    {
        PreferredContactMethod = preferredContactMethod;
        MarketingConsent = marketingConsent;
        NewsletterSubscribed = newsletterSubscribed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignOwner(Guid professionalId)
    {
        OwnerProfessionalId = professionalId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TransferOwnership(Guid newProfessionalId)
    {
        if (OwnerProfessionalId == newProfessionalId)
            throw new InvalidOperationException("Cannot transfer to the same owner.");

        OwnerProfessionalId = newProfessionalId;
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

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new ArgumentException("Tag cannot be empty.", nameof(tag));

        var normalizedTag = tag.Trim().ToLowerInvariant();
        if (!Tags.Contains(normalizedTag))
        {
            Tags.Add(normalizedTag);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveTag(string tag)
    {
        var normalizedTag = tag.Trim().ToLowerInvariant();
        if (Tags.Remove(normalizedTag))
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordActivity()
    {
        LastActivityAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsActive => Status == CustomerStatus.Active;

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Invalid email format.", nameof(email));
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty.", nameof(phone));

        var digitsOnly = new string(phone.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length < 10)
            throw new ArgumentException("Phone number must have at least 10 digits.", nameof(phone));
    }

    private static string NormalizePhone(string phone)
    {
        return new string(phone.Where(char.IsDigit).ToArray());
    }
}
