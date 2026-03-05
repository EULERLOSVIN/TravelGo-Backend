using Application.Interfaces;
using MediatR;

namespace Application.Features.Vehicles.Commands;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, bool>
{
    private readonly IVehicleRepository _repo;

    public CreateVehicleCommandHandler(IVehicleRepository repo)
    {
        _repo = repo;
    }

    public Task<bool> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        => _repo.CreateVehicleAsync(request.Dto);
}