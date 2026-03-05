using Application.Interfaces;
using MediatR;

namespace Application.Features.QueueManagement.Queries.GetDriverQueueInfo;

public class GetDriverQueueInfoQueryHandler : IRequestHandler<GetDriverQueueInfoQuery, GetDriverQueueInfoResponse>
{
    private readonly IQueueManagementRepository _queueRepository;

    public GetDriverQueueInfoQueryHandler(IQueueManagementRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<GetDriverQueueInfoResponse> Handle(GetDriverQueueInfoQuery request, CancellationToken cancellationToken)
    {
        return await _queueRepository.GetDriverQueueInfoByDni(request.Dni);
    }
}
