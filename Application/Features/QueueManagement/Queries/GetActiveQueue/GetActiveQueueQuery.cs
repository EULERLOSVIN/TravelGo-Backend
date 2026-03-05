using MediatR;
using System.Collections.Generic;
using Application.Interfaces;

namespace Application.Features.QueueManagement.Queries.GetActiveQueue;

public record GetActiveQueueQuery : IRequest<List<ActiveQueueDto>>;
