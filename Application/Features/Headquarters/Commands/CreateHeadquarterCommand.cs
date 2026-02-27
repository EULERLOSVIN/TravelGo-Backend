using Application.DTOs.Headquarters;
using Application.Interfaces.Headquarters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Headquarters.Commands
{
    // El Comando recibe el DTO de creación y devuelve un bool (éxito/fracaso) o el ID creado
    public record CreateHeadquarterCommand(CreateHeadquarterDto Dto) : IRequest<bool>;
    public class CreateHeadquarterHandler : IRequestHandler<CreateHeadquarterCommand, bool>
    {
        private readonly IHeadquarterRepository _repository;
        public CreateHeadquarterHandler(IHeadquarterRepository repository) => _repository = repository;
        public async Task<bool> Handle(CreateHeadquarterCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByNameAsync(request.Dto.Name))
            {
                throw new Exception($"Ya existe una sede con el nombre '{request.Dto.Name}'.");
            }
            return await _repository.CreateAsync(request.Dto);
        }
    }
}
