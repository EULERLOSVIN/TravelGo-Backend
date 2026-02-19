using Application.Interfaces.Headquarters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Headquarters.Commands
{
    public record DeleteHeadquarterCommand(int Id) : IRequest<bool>;
    public class DeleteHeadquarterHandler : IRequestHandler<DeleteHeadquarterCommand, bool>
    {
        private readonly IHeadquarterRepository _repository;
        public DeleteHeadquarterHandler(IHeadquarterRepository repository) => _repository = repository;
        public async Task<bool> Handle(DeleteHeadquarterCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }
    }
}
