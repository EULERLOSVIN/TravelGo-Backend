


namespace Application.Interfaces.Driver
{
    public interface IStartingOrderRepository
    {
        Task<int> GetStartingOrderByDriver(int idAccount);
    }
}
