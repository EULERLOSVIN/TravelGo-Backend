using Application.DTOs.Dashboard;
using System.Threading.Tasks;

namespace Application.Interfaces.Dashboard;

public interface IDashboardRepository
{
    Task<DashboardDto> GetDashboardSummaryAsync();
}
