using Application.Common;
using Application.DTOs.ManageSales;
using Application.Interfaces.ManageSales;
using MediatR;



namespace Application.Features.ManageSales.Queries
{
    public record GetSalesByFiltersQuery(FilterOfManageSalesDto Filters) : IRequest<Result<List<SaleDto>>>;

    public class GetSalesByFiltersQueryHandler: IRequestHandler<GetSalesByFiltersQuery, Result<List<SaleDto>>>
    {
        private readonly IGetSalesRepository _getSalesRepository;

        public GetSalesByFiltersQueryHandler(IGetSalesRepository getSalesRepository)
        {
            _getSalesRepository = getSalesRepository;
        }

        public async Task<Result<List<SaleDto>>> Handle(GetSalesByFiltersQuery request, CancellationToken cancellationToken)
        {
           try
           {
               var sales = await _getSalesRepository.GetSalesByFilters(request.Filters);
                if (sales == null)
                {
                    return Result<List<SaleDto>>.Failure("No se encontraron ventas");
                }

                return Result<List<SaleDto>>.Success(sales);
           }

           catch (Exception)
           {
                return Result<List<SaleDto>>.Failure("Error al obtener las ventas");
           }
        }
    }
}
