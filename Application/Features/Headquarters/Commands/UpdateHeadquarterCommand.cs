using Application.DTOs.Headquarters;
using Application.Interfaces.Headquarters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Headquarters.Commands
{
    public record UpdateHeadquarterCommand(int Id, UpdateHeadquarterDto Dto) : IRequest<bool>;
    public class UpdateHeadquarterHandler : IRequestHandler<UpdateHeadquarterCommand, bool>
    {
        private readonly IHeadquarterRepository _repository;
        public UpdateHeadquarterHandler(IHeadquarterRepository repository) => _repository = repository;
        public async Task<bool> Handle(UpdateHeadquarterCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(request.Id, request.Dto);
        }
    }
}
