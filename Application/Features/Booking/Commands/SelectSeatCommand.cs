//using Application.Common;
//using Application.Interfaces.Booking;
//using MediatR;


//namespace Application.Features.Booking.Commands
//{
//    public record SelectSeatCommand(int IdSeatVehicle) : IRequest<Result<bool>>;

//    // 2. El Handler
//    public class SelectSeatCommandHandler : IRequestHandler<SelectSeatCommand, Result<bool>>
//    {
//        private readonly IBookingRepository _repository;

//        public SelectSeatCommandHandler(IBookingRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<Result<bool>> Handle(SelectSeatCommand request, CancellationToken ct)
//        {
//            try
//            {
//                // Paso 1: Bloqueamos el asiento físicamente (Estado 1: Reservado)
//                var success = await _repository.SelectSeat(request.IdSeatVehicle);

//                if (!success)
//                {
//                    return Result<bool>.Failure("El asiento no se pudo reservar o ya está ocupado.");
//                }

//                // Paso 2: Lógica de los 10 minutos (Background Task)
//                // Ejecutamos una tarea en segundo plano que "escuche" el tiempo.
//                // Si en 10 min no se confirma el pago, se libera automáticamente.
//                _ = Task.Run(async () =>
//                {
//                    await Task.Delay(TimeSpan.FromMinutes(10));

//                    // Aquí verificarías si el asiento sigue en estado "Reservado" (1)
//                    // y NO ha pasado a "Vendido" (2).
//                    // Si sigue en 1, lo liberamos.
//                    await _repository.ReleaseSeatIfPending(request.IdSeatVehicle);
//                }, ct);

//                return Result<bool>.Success(true);
//            }
//            catch (Exception)
//            {
//                return Result<bool>.Failure("Error interno al procesar la selección del asiento.");
//            }
//        }
//    }
//}
