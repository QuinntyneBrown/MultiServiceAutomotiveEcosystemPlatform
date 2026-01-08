// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;

public class ProfessionalDto
{
    public Guid ProfessionalId { get; set; }
    public Guid TenantId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Bio { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PhoneBusiness { get; set; }
    public string? Website { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string? ProfilePhotoUrl { get; set; }
    public string? CoverPhotoUrl { get; set; }
    public string? LogoUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Verified { get; set; }
    public bool Featured { get; set; }
    public bool AcceptsReferrals { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
