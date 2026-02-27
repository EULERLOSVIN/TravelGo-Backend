using Application.DTOs.Vehicles;
using MediatR;

namespace Application.Features.Vehicles.Queries;

public record GetVehicleSummaryQuery() : IRequest<VehicleSummaryDto>;
