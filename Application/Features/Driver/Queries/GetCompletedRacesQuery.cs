


using Application.Common;
using Application.DTOs.Driver;
using Application.Interfaces.Driver;
using MediatR;

namespace Application.Features.Driver.Queries
{
    public record GetCompletedRacesQuery(int IdAccount) : IRequest<Result<StatisticsSummaryDriverDto>>;
    public class GetCompetedRacesQueryHandler: IRequestHandler<GetCompletedRacesQuery, Result<StatisticsSummaryDriverDto>>
    {
        private readonly ITripsRepository _tripsRepository;

        public GetCompetedRacesQueryHandler(ITripsRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }

        public async Task<Result<StatisticsSummaryDriverDto>> Handle(GetCompletedRacesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var statistics = await _tripsRepository.GetStatisticsSummaryForDriver(request.IdAccount);
                if (statistics != null)
                {
                    return Result<StatisticsSummaryDriverDto>.Success(statistics);
                }
                else
                {
                    return Result<StatisticsSummaryDriverDto>.Failure("Hubo un error inesperado");
                }
            }
            catch (Exception)
            {
                return Result<StatisticsSummaryDriverDto>.Failure("No se encontraron viajes");
            }
        }
    }
}
