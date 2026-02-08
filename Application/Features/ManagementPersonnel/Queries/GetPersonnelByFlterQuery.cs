using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.ManagementPersonnel.Queries
{
    public record GetPersonnelByFlterQuery(FilterPersonnelDto Fileters) : IRequest<List<PersonnelDto>>;
    public class GetPersonalByFlterHandler : IRequestHandler<GetPersonnelByFlterQuery, List<PersonnelDto>>
    {
        private readonly IGetPersonnelRepository _personnelRepository;
        public GetPersonalByFlterHandler(IGetPersonnelRepository personnelRepository)
        {
            _personnelRepository = personnelRepository;
        }
        public async Task<List<PersonnelDto>> Handle(GetPersonnelByFlterQuery request, CancellationToken cancellationToken)
        {
            return await _personnelRepository.GetPersonnelByFilters(request.Fileters);
        }
    }
}
