using Application.DTOs.Headquarters;
using Application.Interfaces.Headquarters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Headquarters.Queries
{
    public record GetHeadquarterByIdQuery(int Id) : IRequest<HeadquarterDto?>;
    public class GetHeadquarterByIdHandler : IRequestHandler<GetHeadquarterByIdQuery, HeadquarterDto?>
    {
        private readonly IHeadquarterRepository _repository;
        public GetHeadquarterByIdHandler(IHeadquarterRepository repository) => _repository = repository;
        public async Task<HeadquarterDto?> Handle(GetHeadquarterByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
