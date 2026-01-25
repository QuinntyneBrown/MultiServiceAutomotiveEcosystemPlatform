using MediatR;
using Professional.Core.Interfaces;
using Shared.Contracts.DTOs;

namespace Professional.Api.Queries.GetSpecialties;

public class GetSpecialtiesQueryHandler : IRequestHandler<GetSpecialtiesQuery, IReadOnlyList<SpecialtyCatalogDto>>
{
    private readonly ISpecialtyCatalogRepository _repository;

    public GetSpecialtiesQueryHandler(ISpecialtyCatalogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SpecialtyCatalogDto>> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await _repository.GetActiveAsync(cancellationToken);

        return specialties.Select(s => new SpecialtyCatalogDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Category = s.Category,
            IsActive = s.IsActive
        }).ToList();
    }
}
