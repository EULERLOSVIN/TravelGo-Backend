
using Application.Common;
using Application.DTOs.Driver;
using Application.Interfaces.Driver;
using MediatR;

namespace Application.Features.Driver.Queries
{
    public record GetTripsMadeQuery(int IdAccount, int filterOption) : IRequest<Result<List<TripsMadeDto>>>;

    public class GetTripsMadeQueryHandler: IRequestHandler<GetTripsMadeQuery, Result<List<TripsMadeDto>>>
    {
        public readonly ITripsRepository _tripsRepository;
        public GetTripsMadeQueryHandler(ITripsRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }

        public async Task<Result<List<TripsMadeDto>>> Handle(GetTripsMadeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var  trips = await _tripsRepository.GetTripsMade(request.IdAccount, request.filterOption);
                if (trips != null)
                {
                    return Result<List<TripsMadeDto>>.Success(trips);
                }
                else
                {
                    return Result<List<TripsMadeDto>>.Failure("No se encontraron viajes");
                }
            }
            catch (Exception)
            {
                return Result<List<TripsMadeDto>>.Failure("No se encontraron viajes");
            }
        }
    }
}
