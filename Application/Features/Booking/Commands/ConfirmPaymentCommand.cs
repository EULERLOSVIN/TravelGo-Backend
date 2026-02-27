using Application.Common;
using Application.Interfaces.Booking;
using MediatR;

namespace Application.Features.Booking.Commands
{
    public record ConfirmPaymentCommand(int IdSeatVehicle, string OperationCode) : IRequest<Result<bool>>;

    public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, Result<bool>>
    {
        private readonly IBookingRepository _repository;

        public ConfirmPaymentHandler(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(ConfirmPaymentCommand request, CancellationToken ct)
        {
            try
            {
                var success = await _repository.ConfirmPayment(request.IdSeatVehicle);

                if (!success)
                    return Result<bool>.Failure("No se pudo confirmar el pago. Verifique el estado del asiento.");

                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                return Result<bool>.Failure("Error al procesar el pago final.");
            }
        }
    }
}
