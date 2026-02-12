using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.ManagementPersonnel.Queries
{
    public record GetStatsPersonnelQuery : IRequest<StatsUsersDto>;
    public class GetStatsPersonnelHandler: IRequestHandler<GetStatsPersonnelQuery, StatsUsersDto>
    {
        private readonly IGetPersonnelStatisticsRepository _statisticsPersonnelRepository;

        public GetStatsPersonnelHandler(IGetPersonnelStatisticsRepository statisticsPersonnelRepository)
        {
            _statisticsPersonnelRepository = statisticsPersonnelRepository;
        }

        public async Task<StatsUsersDto> Handle(GetStatsPersonnelQuery request, CancellationToken cancellationToken)
        {
            return await _statisticsPersonnelRepository.GetStatsUsers();
        }
    }
}
