

using Application.Common;
using Application.DTOs.Headquarters;
using Application.Interfaces.Booking;
using MediatR;

namespace Application.Features.Booking.Queries
{
    public record GetSeatByIdOfVehicleQuery (int idVehicle) : IRequest<Result<List<SeatDto>>>;

    public class GetSeatByIdOfVehicleQueryHandler: IRequestHandler<GetSeatByIdOfVehicleQuery,Result<List<SeatDto>>>
    {
        public readonly IGetSeatRepository _getSeatRepository;

        public GetSeatByIdOfVehicleQueryHandler(IGetSeatRepository getSeatRepository)
        {
            _getSeatRepository = getSeatRepository;
        }

        public async Task<Result<List<SeatDto>>> Handle(GetSeatByIdOfVehicleQuery request, CancellationToken cancellationToken)
        {          
            try
            {
                var seats = await _getSeatRepository.GetSeatByIdOfVehicle(request.idVehicle);
                if (seats == null)
                {
                    return Result<List<SeatDto>>.Failure("No se encontraron asientos disponibles.");
                }
                return Result<List<SeatDto>>.Success(seats);
            }
            catch (Exception)
            {
                return Result<List<SeatDto>>.Failure("Ocurrio un error al obtener los asientos.");
            }  
        }
    }
}
