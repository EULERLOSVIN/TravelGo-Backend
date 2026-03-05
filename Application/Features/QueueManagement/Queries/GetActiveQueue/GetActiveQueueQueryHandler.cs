using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.QueueManagement.Queries.GetActiveQueue;

public class GetActiveQueueQueryHandler : IRequestHandler<GetActiveQueueQuery, List<ActiveQueueDto>>
{
    private readonly IQueueManagementRepository _repository;

    public GetActiveQueueQueryHandler(IQueueManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ActiveQueueDto>> Handle(GetActiveQueueQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetActiveQueue();
    }
}
