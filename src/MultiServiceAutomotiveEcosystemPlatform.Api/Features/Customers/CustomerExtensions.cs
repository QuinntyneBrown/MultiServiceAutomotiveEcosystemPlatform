// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;

public static class CustomerExtensions
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            TenantId = customer.TenantId,
            OwnerProfessionalId = customer.OwnerProfessionalId,
            Email = customer.Email,
            Phone = customer.Phone,
            PhoneSecondary = customer.PhoneSecondary,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth,
            AddressLine1 = customer.AddressLine1,
            AddressLine2 = customer.AddressLine2,
            City = customer.City,
            Province = customer.Province,
            PostalCode = customer.PostalCode,
            Country = customer.Country,
            PreferredContactMethod = customer.PreferredContactMethod,
            MarketingConsent = customer.MarketingConsent,
            NewsletterSubscribed = customer.NewsletterSubscribed,
            Status = customer.Status,
            EmailVerified = customer.EmailVerified,
            PhoneVerified = customer.PhoneVerified,
            Source = customer.Source,
            Notes = customer.Notes,
            Tags = new List<string>(customer.Tags),
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            LastActivityAt = customer.LastActivityAt
        };
    }
}
