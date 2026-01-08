// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;

public class CustomerDto
{
    public Guid CustomerId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? OwnerProfessionalId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PhoneSecondary { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string Country { get; set; } = "CA";
    public PreferredContactMethod PreferredContactMethod { get; set; }
    public bool MarketingConsent { get; set; }
    public bool NewsletterSubscribed { get; set; }
    public CustomerStatus Status { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public string? Source { get; set; }
    public string? Notes { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastActivityAt { get; set; }
}
