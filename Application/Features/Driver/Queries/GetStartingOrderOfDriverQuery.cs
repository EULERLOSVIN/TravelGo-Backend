
using Application.Common;
using Application.Interfaces.Driver;
using MediatR;

namespace Application.Features.Driver.Queries
{
    public record GetStartingOrderOfDriverQuery(int IdAccount) : IRequest<Result<int>>;
    public class GetStartingOrderOfDriverQueryHandler: IRequestHandler<GetStartingOrderOfDriverQuery, Result<int>>
    {
        private readonly IStartingOrderRepository _tripsRepository;

        public GetStartingOrderOfDriverQueryHandler(IStartingOrderRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }
        public async Task<Result<int>> Handle(GetStartingOrderOfDriverQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var startingOrder = await _tripsRepository.GetStartingOrderByDriver(request.IdAccount);
                if (startingOrder == 0)
                {
                    return Result<int>.Failure("No se encontraron viajes");
                }
                else
                {
                    return Result<int>.Success(startingOrder);
                }
            }
            catch (Exception)
            {
                return Result<int>.Failure("problemas en el servidor");
            }
        }
    }
}
