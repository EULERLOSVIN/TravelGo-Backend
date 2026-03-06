using Application.DTOs.QueueVehicles;
using MediatR;
using Application.Interfaces.QueueVehicles;

namespace Application.Features.QueueVehicles.Queries.GetDriverQueueInfo
{
    public class GetDriverQueueInfoQuery : IRequest<DriverQueueInfoDto?>
    {
        public string Dni { get; set; }

        public GetDriverQueueInfoQuery(string dni)
        {
            Dni = dni;
        }
    }

    public class GetDriverQueueInfoQueryHandler : IRequestHandler<GetDriverQueueInfoQuery, DriverQueueInfoDto?>
    {
        private readonly IGetDriverQueueInfoRepository _repository;

        public GetDriverQueueInfoQueryHandler(IGetDriverQueueInfoRepository repository)
        {
            _repository = repository;
        }

        public async Task<DriverQueueInfoDto?> Handle(GetDriverQueueInfoQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetDriverQueueInfoAsync(request.Dni);
        }
    }
}
