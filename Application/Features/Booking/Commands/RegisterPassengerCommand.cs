//using Application.Common;
//using Application.Interfaces.Booking;
//using MediatR;


//namespace Application.Features.Booking.Commands
//{
//    public record RegisterPassengerCommand(
//        int IdSeatVehicle,
//        string Dni,
//        string FullName,
//        string Telephone,
//        string Email,
//        string PickUpPoint
//    ) : IRequest<Result<bool>>;

//    public class RegisterPassengerHandler : IRequestHandler<RegisterPassengerCommand, Result<bool>>
//    {
//        private readonly IBookingRepository _repository;

//        public RegisterPassengerHandler(IBookingRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<Result<bool>> Handle(RegisterPassengerCommand request, CancellationToken ct)
//        {
//            try
//            {
//                // Validación de negocio básica
//                if (string.IsNullOrEmpty(request.Dni) || string.IsNullOrEmpty(request.FullName))
//                {
//                    return Result<bool>.Failure("El DNI y el nombre completo son obligatorios.");
//                }

//                var success = await _repository.UpdatePassengerDetails(
//                    request.IdSeatVehicle,
//                    request.Dni,
//                    request.FullName,
//                    request.PickUpPoint
//                );

//                if (!success)
//                {
//                    return Result<bool>.Failure("No se pudo registrar la información del pasajero. Verifique si el asiento aún está reservado.");
//                }

//                return Result<bool>.Success(true);
//            }
//            catch (Exception ex)
//            {
//                return Result<bool>.Failure("Ocurrió un error inesperado al registrar al pasajero.");
//            }
//        }
//    }
//}
