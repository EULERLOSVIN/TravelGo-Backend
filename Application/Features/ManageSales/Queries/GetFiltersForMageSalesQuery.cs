

using Application.Common;
using Application.DTOs.ManageSales;
using Application.Interfaces.ManageSales;
using MediatR;

namespace Application.Features.ManageSales.Queries
{
    public record GetFiltersForMageSalesQuery(): IRequest<Result<FiltersDto>>;
    public class GetFiltersForMageSalesQueryHandler: IRequestHandler<GetFiltersForMageSalesQuery, Result<FiltersDto> >
    {
        public IGetFilterRepository _getFilterRepository { get; set; }
        public GetFiltersForMageSalesQueryHandler(IGetFilterRepository getFilterRepository)
        {
            _getFilterRepository = getFilterRepository;
        }
        public async Task<Result<FiltersDto>> Handle(GetFiltersForMageSalesQuery request, CancellationToken cancellationToken)
        {
            var filters = await _getFilterRepository.GetFilterForMageSales();
            if (filters == null)
            {
                return Result<FiltersDto>.Failure("No se encontraron filtros");
            }
            return Result<FiltersDto>.Success(filters);
        }
    }
}
