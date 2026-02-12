

using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.ManagementPersonnel.Queries
{
    public record GetUserByIdAccountQuery(int id) : IRequest<PersonnelDto>;
    public class GetUserByIdAccountHandler : IRequestHandler<GetUserByIdAccountQuery, PersonnelDto>
    {
        private readonly IGetUserRepository _personnelRepository;
        public GetUserByIdAccountHandler(IGetUserRepository personnelRepository)
        {
            _personnelRepository = personnelRepository;
        }
        public Task<PersonnelDto> Handle(GetUserByIdAccountQuery request, CancellationToken cancellationToken)
        {
            return _personnelRepository.GetPersonnelByIdAccount(request.id);
        }
    }
}
