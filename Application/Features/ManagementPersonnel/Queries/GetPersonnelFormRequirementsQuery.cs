using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ManagementPersonnel.Queries
{
    public record GetPersonnelFormRequirementsQuery: IRequest<PersonnelFormRequirementsDto>;
    public class GetPersonnelFormRequirementsHandler: IRequestHandler<GetPersonnelFormRequirementsQuery, PersonnelFormRequirementsDto>
    {
        private readonly IGetAllRolesRepository _rolesRepository;
        private readonly IGetTypesDocumentsRepository _tipesDocumentsRepository;
        private readonly IGetStatesAccountRepository _statesAccountsRepository;
        public GetPersonnelFormRequirementsHandler(IGetAllRolesRepository rolesRepository, IGetTypesDocumentsRepository tipesDocumentsRepository, IGetStatesAccountRepository statesAccountsRepository)
        {
            _rolesRepository = rolesRepository;
            _tipesDocumentsRepository = tipesDocumentsRepository;
            _statesAccountsRepository = statesAccountsRepository;
        }

        public async Task<PersonnelFormRequirementsDto> Handle(GetPersonnelFormRequirementsQuery request, CancellationToken cancellationToken)
        {
            return new PersonnelFormRequirementsDto()
            {
                Roles = await _rolesRepository.GetAllRoles(),
                DocumentTypes = await _tipesDocumentsRepository.GetTypesDocuments(),
                StateOfAccount = await _statesAccountsRepository.GetStatesAccount()
            };
        }
    }
}
