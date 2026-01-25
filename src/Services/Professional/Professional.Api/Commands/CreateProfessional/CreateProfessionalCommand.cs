using MediatR;
using Shared.Contracts.DTOs;

namespace Professional.Api.Commands.CreateProfessional;

public record CreateProfessionalCommand(
    string BusinessName,
    string? BusinessDescription,
    string? BusinessPhone,
    string? BusinessEmail,
    string? Website,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? StreetAddress,
    string? City,
    string? State,
    string? PostalCode,
    string? Country,
    double? Latitude,
    double? Longitude,
    bool AcceptsReferrals,
    decimal? ReferralCommissionRate
) : IRequest<ProfessionalDto>;
