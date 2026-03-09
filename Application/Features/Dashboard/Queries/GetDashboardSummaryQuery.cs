using Application.Common;
using Application.DTOs.Dashboard;
using Application.Interfaces.Dashboard;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Dashboard.Queries;

public record GetDashboardSummaryQuery() : IRequest<Result<DashboardDto>>;

public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardDto>>
{
    private readonly IDashboardRepository _dashboardRepository;

    public GetDashboardSummaryQueryHandler(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<Result<DashboardDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dashboardRepository.GetDashboardSummaryAsync();
            return Result<DashboardDto>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<DashboardDto>.Failure($"Error al obtener resumen del dashboard: {ex.Message}");
        }
    }
}
