

using Application.DTOs.ManageSales;

namespace Application.Interfaces.ManageSales
{
    public interface IGetSalesRepository
    {
        Task<List<SaleDto>?> GetSalesByFilters(FilterOfManageSalesDto filters);
    }
}
