using MediatR;
using Application.Interfaces.DepartureTimes;

namespace Application.Features.DepartureTimes.Commands
{
    public class DeleteDepartureTimeCommand : IRequest<bool>
    {
        public int IdDepartureTime { get; }
        public DeleteDepartureTimeCommand(int idDepartureTime) => IdDepartureTime = idDepartureTime;
    }

    public class DeleteDepartureTimeCommandHandler : IRequestHandler<DeleteDepartureTimeCommand, bool>
    {
        private readonly IDeleteDepartureTimeRepository _repository;

        public DeleteDepartureTimeCommandHandler(IDeleteDepartureTimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteDepartureTimeCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteDepartureTimeAsync(request.IdDepartureTime);
        }
    }
}
