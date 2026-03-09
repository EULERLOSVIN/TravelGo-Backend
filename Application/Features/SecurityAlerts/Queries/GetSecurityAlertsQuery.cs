using Application.Common;
using Application.DTOs.SecurityAlerts;
using Application.Interfaces.SecurityAlerts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SecurityAlerts.Queries;

public record GetSecurityAlertsQuery() : IRequest<Result<SecurityAlertsDto>>;

public class GetSecurityAlertsQueryHandler : IRequestHandler<GetSecurityAlertsQuery, Result<SecurityAlertsDto>>
{
    private readonly IGetSecurityAlertsRepository _securityAlertsRepository;

    public GetSecurityAlertsQueryHandler(IGetSecurityAlertsRepository securityAlertsRepository)
    {
        _securityAlertsRepository = securityAlertsRepository;
    }

    public async Task<Result<SecurityAlertsDto>> Handle(GetSecurityAlertsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _securityAlertsRepository.GetExpiringDocumentsAsync();
            return Result<SecurityAlertsDto>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<SecurityAlertsDto>.Failure($"Error al obtener alertas de seguridad: {ex.Message}");
        }
    }
}
