using Application.DTOs.Headquarters;
using Application.Interfaces.Headquarters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Headquarters.Queries
{
    // La "Pregunta" (Query) devuelve una lista de DTOs
    public record GetAllHeadquartersQuery : IRequest<List<HeadquarterDto>>;
    // El "Manejador" (Handler) responde la pregunta usando el Repositorio
    public class GetAllHeadquartersHandler : IRequestHandler<GetAllHeadquartersQuery, List<HeadquarterDto>>
    {
        private readonly IHeadquarterRepository _repository;
        public GetAllHeadquartersHandler(IHeadquarterRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<HeadquarterDto>> Handle(GetAllHeadquartersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
