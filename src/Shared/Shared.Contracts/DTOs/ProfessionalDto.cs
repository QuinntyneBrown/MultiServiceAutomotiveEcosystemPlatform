using Shared.Contracts.Enums;

namespace Shared.Contracts.DTOs;

public record ProfessionalDto
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public string BusinessName { get; init; } = string.Empty;
    public string? BusinessDescription { get; init; }
    public string? BusinessPhone { get; init; }
    public string? BusinessEmail { get; init; }
    public string? Website { get; init; }
    public string Slug { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string? CoverImageUrl { get; init; }
    public ProfessionalStatus Status { get; init; }
    public bool IsVerified { get; init; }
    public bool IsFeatured { get; init; }
    public bool AcceptsReferrals { get; init; }
    public decimal? ReferralCommissionRate { get; init; }
    public IReadOnlyList<ProfessionalSpecialtyDto> Specialties { get; init; } = [];
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record ProfessionalSpecialtyDto
{
    public Guid Id { get; init; }
    public Guid ProfessionalId { get; init; }
    public Guid SpecialtyId { get; init; }
    public string SpecialtyName { get; init; } = string.Empty;
    public string? Certification { get; init; }
    public bool IsVerified { get; init; }
    public int YearsOfExperience { get; init; }
}

public record SpecialtyCatalogDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Category { get; init; }
    public bool IsActive { get; init; }
}

public record CreateProfessionalRequest
{
    public string BusinessName { get; init; } = string.Empty;
    public string? BusinessDescription { get; init; }
    public string? BusinessPhone { get; init; }
    public string? BusinessEmail { get; init; }
    public string? Website { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public bool AcceptsReferrals { get; init; } = true;
    public decimal? ReferralCommissionRate { get; init; }
}

public record UpdateProfessionalRequest
{
    public string BusinessName { get; init; } = string.Empty;
    public string? BusinessDescription { get; init; }
    public string? BusinessPhone { get; init; }
    public string? BusinessEmail { get; init; }
    public string? Website { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string? CoverImageUrl { get; init; }
    public bool AcceptsReferrals { get; init; }
    public decimal? ReferralCommissionRate { get; init; }
}

public record AddSpecialtyRequest
{
    public Guid SpecialtyId { get; init; }
    public string? Certification { get; init; }
    public int YearsOfExperience { get; init; }
}

public record CreateSpecialtyRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Category { get; init; }
}

public record UpdateProfessionalStatusRequest
{
    public ProfessionalStatus Status { get; init; }
}
