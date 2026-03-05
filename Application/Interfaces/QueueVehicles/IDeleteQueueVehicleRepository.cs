namespace Application.Interfaces.QueueVehicles
{
    public interface IDeleteQueueVehicleRepository
    {
        Task<bool> DeleteQueueVehicleAsync(int idAssignQueue);
    }
}
