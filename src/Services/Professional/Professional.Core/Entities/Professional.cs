using System.Text.RegularExpressions;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Professional.Core.Entities;

public partial class Professional : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }

    // Business Information
    public string BusinessName { get; private set; } = string.Empty;
    public string? BusinessDescription { get; private set; }
    public string? BusinessPhone { get; private set; }
    public string? BusinessEmail { get; private set; }
    public string? Website { get; private set; }
    public string Slug { get; private set; } = string.Empty;

    // Personal Information
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }

    // Address
    public string? StreetAddress { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Country { get; private set; }

    // Location
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }

    // Media
    public string? ProfileImageUrl { get; private set; }
    public string? CoverImageUrl { get; private set; }

    // Status
    public ProfessionalStatus Status { get; private set; }
    public bool IsVerified { get; private set; }
    public bool IsFeatured { get; private set; }

    // Referral Settings
    public bool AcceptsReferrals { get; private set; }
    public decimal? ReferralCommissionRate { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation
    private readonly List<ProfessionalSpecialty> _specialties = [];
    public IReadOnlyCollection<ProfessionalSpecialty> Specialties => _specialties.AsReadOnly();

    private Professional() { }

    public Professional(
        Guid tenantId,
        string businessName,
        string firstName,
        string lastName,
        string email)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name is required", nameof(businessName));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        BusinessName = businessName.Trim();
        Slug = GenerateSlug(businessName);
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        Status = ProfessionalStatus.Pending;
        AcceptsReferrals = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateBusinessInfo(
        string businessName,
        string? businessDescription,
        string? businessPhone,
        string? businessEmail,
        string? website)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name is required", nameof(businessName));

        BusinessName = businessName.Trim();
        Slug = GenerateSlug(businessName);
        BusinessDescription = businessDescription?.Trim();
        BusinessPhone = businessPhone?.Trim();
        BusinessEmail = businessEmail?.Trim().ToLowerInvariant();
        Website = website?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string email, string? phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        Phone = phone?.Trim();
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

    public void UpdateLocation(double? latitude, double? longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMedia(string? profileImageUrl, string? coverImageUrl)
    {
        ProfileImageUrl = profileImageUrl?.Trim();
        CoverImageUrl = coverImageUrl?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateReferralSettings(bool acceptsReferrals, decimal? commissionRate)
    {
        AcceptsReferrals = acceptsReferrals;
        ReferralCommissionRate = commissionRate;
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
        IsVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured)
    {
        IsFeatured = featured;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddSpecialty(Guid specialtyId, string specialtyName, string? certification, int yearsOfExperience)
    {
        if (_specialties.Any(s => s.SpecialtyId == specialtyId))
            return;

        var specialty = new ProfessionalSpecialty(Id, specialtyId, specialtyName, certification, yearsOfExperience);
        _specialties.Add(specialty);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveSpecialty(Guid specialtyId)
    {
        var specialty = _specialties.FirstOrDefault(s => s.SpecialtyId == specialtyId);
        if (specialty is not null)
        {
            _specialties.Remove(specialty);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    private static string GenerateSlug(string name)
    {
        var slug = name.ToLowerInvariant();
        slug = SlugRegex().Replace(slug, "-");
        slug = MultipleHyphensRegex().Replace(slug, "-");
        slug = slug.Trim('-');
        return slug;
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex SlugRegex();

    [GeneratedRegex(@"-+")]
    private static partial Regex MultipleHyphensRegex();
}
