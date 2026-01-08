// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;

public static class ProfessionalExtensions
{
    public static ProfessionalDto ToDto(this Professional professional)
    {
        return new ProfessionalDto
        {
            ProfessionalId = professional.ProfessionalId,
            TenantId = professional.TenantId,
            BusinessName = professional.BusinessName,
            BusinessType = professional.BusinessType.ToString(),
            FirstName = professional.FirstName,
            LastName = professional.LastName,
            FullName = professional.FullName,
            Title = professional.Title,
            Bio = professional.Bio,
            Email = professional.Email,
            Phone = professional.Phone,
            PhoneBusiness = professional.PhoneBusiness,
            Website = professional.Website,
            City = professional.City,
            Province = professional.Province,
            PostalCode = professional.PostalCode,
            ProfilePhotoUrl = professional.ProfilePhotoUrl,
            CoverPhotoUrl = professional.CoverPhotoUrl,
            LogoUrl = professional.LogoUrl,
            Status = professional.Status.ToString(),
            Verified = professional.Verified,
            Featured = professional.Featured,
            AcceptsReferrals = professional.AcceptsReferrals,
            Slug = professional.Slug,
            CreatedAt = professional.CreatedAt,
            UpdatedAt = professional.UpdatedAt
        };
    }
}
