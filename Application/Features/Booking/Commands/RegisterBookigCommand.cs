using Application.Common;
using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using MediatR;

namespace Application.Features.Booking.Commands
{
    public record RegisterBookigCommand(RegisterBookingDto dataBooking) : IRequest<Result<bool>>;
    public class RegisterBookigCommandHandler: IRequestHandler<RegisterBookigCommand, Result<bool>>
    {
        private readonly IBookingRepository _repository;
        public RegisterBookigCommandHandler(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(RegisterBookigCommand request, CancellationToken cacellationToken)
        {
           
            try
            {
                bool response = await _repository.RegisterBooking(request.dataBooking);
                if (!response)
                {
                    return Result<bool>.Failure("Error al registrar la reserva.");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                return Result<bool>.Failure("Error al registrar la reserva.");
            }
        }
    }
}
