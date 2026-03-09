using Application.Common;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands
{
    public record RegisterArrivalCommand(string DriverDni) : IRequest<Result<bool>>;

    public class RegisterArrivalHandler : IRequestHandler<RegisterArrivalCommand, Result<bool>>
    {
        private readonly IRegisterArrivalRepository _repository;

        public RegisterArrivalHandler(IRegisterArrivalRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(RegisterArrivalCommand request, CancellationToken cancellationToken)
        {
            return await _repository.RegisterArrivalAsync(request.DriverDni);
        }
    }
}
