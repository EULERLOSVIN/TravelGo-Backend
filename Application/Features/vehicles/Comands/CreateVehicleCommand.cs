using Application.DTOs.Vehicles;
using MediatR;

namespace Application.Features.Vehicles.Commands;

public record CreateVehicleCommand(CreateVehicleDto Dto) : IRequest<bool>;