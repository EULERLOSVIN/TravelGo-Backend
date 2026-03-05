using Application.Features.QueueManagement.Queries.GetDriverQueueInfo;

namespace Application.Interfaces.QueueManagement;

public interface IQueueManagementRepository
{
    Task<GetDriverQueueInfoResponse> GetDriverQueueInfoByDni(string dni);
    Task<int> AddQueueVehicle(int idVehicle);
    Task<List<ActiveQueueDto>> GetActiveQueue();
    Task<bool> DeleteQueueVehicle(int idQueueVehicle);
}

public class ActiveQueueDto
{
    public int IdQueueVehicle { get; set; }
    public int Number { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public DateTime EntryTime { get; set; }
}
