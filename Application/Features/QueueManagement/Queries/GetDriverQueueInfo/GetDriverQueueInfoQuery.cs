using Application.Features.QueueManagement.Queries.GetDriverQueueInfo;
using MediatR;

namespace Application.Features.QueueManagement.Queries.GetDriverQueueInfo;

public class GetDriverQueueInfoQuery : IRequest<GetDriverQueueInfoResponse>
{
    public string Dni { get; set; } = string.Empty;
}
