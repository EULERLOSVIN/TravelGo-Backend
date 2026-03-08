using MediatR;
using Application.DTOs.DepartureTimes;
using Application.Interfaces.DepartureTimes;

namespace Application.Features.DepartureTimes.Commands
{
    public class AddDepartureTimeCommand : IRequest<int>
    {
        public AddDepartureTimeDto Dto { get; }
        public AddDepartureTimeCommand(AddDepartureTimeDto dto) => Dto = dto;
    }

    public class AddDepartureTimeCommandHandler : IRequestHandler<AddDepartureTimeCommand, int>
    {
        private readonly IAddDepartureTimeRepository _repository;

        public AddDepartureTimeCommandHandler(IAddDepartureTimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddDepartureTimeCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddDepartureTimeAsync(request.Dto);
        }
    }
}
