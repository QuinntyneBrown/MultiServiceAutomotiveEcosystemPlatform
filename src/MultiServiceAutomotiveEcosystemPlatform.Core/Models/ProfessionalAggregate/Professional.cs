using System.Text.RegularExpressions;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

public partial class Professional
{
    public Guid ProfessionalId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }

    // Business Information
    public string BusinessName { get; private set; } = string.Empty;
    public BusinessType BusinessType { get; private set; }
    public string? LicenseNumber { get; private set; }
    public string? TaxId { get; private set; }

    // Personal Information
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? Title { get; private set; }
    public string? Bio { get; private set; }

    // Contact Information
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string? PhoneBusiness { get; private set; }
    public string? Website { get; private set; }

    // Address
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string Province { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = "CA";

    // Location
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public int? ServiceRadiusMiles { get; private set; }

    // Media
    public string? ProfilePhotoUrl { get; private set; }
    public string? CoverPhotoUrl { get; private set; }
    public string? LogoUrl { get; private set; }

    // Status
    public ProfessionalStatus Status { get; private set; }
    public bool Verified { get; private set; }
    public bool Featured { get; private set; }

    // Settings
    public bool AcceptsReferrals { get; private set; }
    public bool AutoAcceptInquiries { get; private set; }
    public string? NotificationPreferences { get; private set; }

    // URL Slug
    public string Slug { get; private set; } = string.Empty;

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? VerifiedAt { get; private set; }

    // Navigation
    public ICollection<ProfessionalSpecialty> Specialties { get; private set; } = new List<ProfessionalSpecialty>();

    private Professional() { }

    public Professional(
        Guid tenantId,
        Guid userId,
        string businessName,
        BusinessType businessType,
        string firstName,
        string lastName,
        string email,
        string phone,
        string addressLine1,
        string city,
        string province,
        string postalCode)
    {
        ValidateRequiredFields(businessName, firstName, lastName, email, phone, addressLine1, city, province, postalCode);

        ProfessionalId = Guid.NewGuid();
        TenantId = tenantId;
        UserId = userId;
        BusinessName = businessName.Trim();
        BusinessType = businessType;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.ToLowerInvariant().Trim();
        Phone = NormalizePhone(phone);
        AddressLine1 = addressLine1.Trim();
        City = city.Trim();
        Province = province.Trim();
        PostalCode = postalCode.Trim();
        Status = ProfessionalStatus.Pending;
        AcceptsReferrals = true;
        Slug = GenerateSlug(businessName);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBusinessInfo(
        string businessName,
        BusinessType businessType,
        string? licenseNumber,
        string? taxId)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name cannot be empty.", nameof(businessName));

        BusinessName = businessName.Trim();
        BusinessType = businessType;
        LicenseNumber = licenseNumber?.Trim();
        TaxId = taxId?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string? title, string? bio)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Title = title?.Trim();
        Bio = bio?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string email, string phone, string? phoneBusiness, string? website)
    {
        ValidateEmail(email);
        ValidatePhone(phone);

        Email = email.ToLowerInvariant().Trim();
        Phone = NormalizePhone(phone);
        PhoneBusiness = phoneBusiness != null ? NormalizePhone(phoneBusiness) : null;
        Website = website?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(
        string addressLine1,
        string? addressLine2,
        string city,
        string province,
        string postalCode,
        string? country = null)
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty.", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));
        if (string.IsNullOrWhiteSpace(province))
            throw new ArgumentException("Province cannot be empty.", nameof(province));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty.", nameof(postalCode));

        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2?.Trim();
        City = city.Trim();
        Province = province.Trim();
        PostalCode = postalCode.Trim();
        if (!string.IsNullOrWhiteSpace(country))
            Country = country.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLocation(decimal? latitude, decimal? longitude, int? serviceRadiusMiles)
    {
        Latitude = latitude;
        Longitude = longitude;
        ServiceRadiusMiles = serviceRadiusMiles;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMedia(string? profilePhotoUrl, string? coverPhotoUrl, string? logoUrl)
    {
        ProfilePhotoUrl = profilePhotoUrl?.Trim();
        CoverPhotoUrl = coverPhotoUrl?.Trim();
        LogoUrl = logoUrl?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSettings(bool acceptsReferrals, bool autoAcceptInquiries, string? notificationPreferences)
    {
        AcceptsReferrals = acceptsReferrals;
        AutoAcceptInquiries = autoAcceptInquiries;
        NotificationPreferences = notificationPreferences;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = ProfessionalStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = ProfessionalStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = ProfessionalStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Verify()
    {
        Verified = true;
        VerifiedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured)
    {
        Featured = featured;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddSpecialty(ProfessionalSpecialty specialty)
    {
        Specialties.Add(specialty);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveSpecialty(ProfessionalSpecialty specialty)
    {
        Specialties.Remove(specialty);
        UpdatedAt = DateTime.UtcNow;
    }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsActive => Status == ProfessionalStatus.Active;

    private static void ValidateRequiredFields(
        string businessName,
        string firstName,
        string lastName,
        string email,
        string phone,
        string addressLine1,
        string city,
        string province,
        string postalCode)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name cannot be empty.", nameof(businessName));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        ValidateEmail(email);
        ValidatePhone(phone);
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty.", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));
        if (string.IsNullOrWhiteSpace(province))
            throw new ArgumentException("Province cannot be empty.", nameof(province));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty.", nameof(postalCode));
    }

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

    private static string GenerateSlug(string businessName)
    {
        var slug = businessName.ToLowerInvariant().Trim();
        slug = SlugRegex().Replace(slug, "-");
        slug = MultiDashRegex().Replace(slug, "-");
        slug = slug.Trim('-');
        return slug;
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex SlugRegex();

    [GeneratedRegex("-+")]
    private static partial Regex MultiDashRegex();
}
