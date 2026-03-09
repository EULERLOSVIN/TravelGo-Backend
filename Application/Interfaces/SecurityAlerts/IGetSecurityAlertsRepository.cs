using Application.DTOs.SecurityAlerts;
using System.Threading.Tasks;

namespace Application.Interfaces.SecurityAlerts;

public interface IGetSecurityAlertsRepository
{
    Task<SecurityAlertsDto> GetExpiringDocumentsAsync();
}
