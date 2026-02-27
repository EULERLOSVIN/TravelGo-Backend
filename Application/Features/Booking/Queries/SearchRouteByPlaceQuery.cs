using Application.Common;
using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using MediatR;

namespace Application.Features.Booking.Queries
{
    public record SearchRouteByPlaceQuery(SearchTravelRouteDto searchTravelDto) : IRequest<Result<DataRouteDto>>;
    public class SearchRouteByPlaceQueryHandler : IRequestHandler<SearchRouteByPlaceQuery, Result<DataRouteDto>>
    {
        private readonly ISearchRouteRepository _searchRouteRepository;
        public SearchRouteByPlaceQueryHandler(ISearchRouteRepository searchRouteRepository)
        {
            _searchRouteRepository = searchRouteRepository;
        }
        public async Task<Result<DataRouteDto>> Handle(SearchRouteByPlaceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                DataRouteDto? result;

                // Decidimos el método basándonos en la opción del día
                if (request.searchTravelDto.DayOption == 1)
                {
                    // Llamamos al método de mañana
                    result = await _searchRouteRepository.SearchRouteByPlaceTomorrow(request.searchTravelDto);
                }
                else
                {
                    // Por defecto (0 o cualquier otro) llamamos al de hoy
                    result = await _searchRouteRepository.SearchRouteByPlaceToday(request.searchTravelDto);
                }

                if (result == null)
                {
                    return Result<DataRouteDto>.Failure("No se encontraron rutas disponibles para el trayecto seleccionado.");
                }

                return Result<DataRouteDto>.Success(result);
            }
            catch (Exception ex)
            {
                // Es buena práctica loguear la excepción interna aquí si tienes un logger
                return Result<DataRouteDto>.Failure("Ocurrió un problema inesperado al buscar la ruta.");
            }
        }
    }
}
