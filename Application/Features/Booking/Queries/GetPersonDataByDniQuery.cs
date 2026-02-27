using Application.Common;
using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using MediatR;

namespace Application.Features.Booking.Queries
{
    public record GetPersonDataByDniQuery(int dni): IRequest<Result<PersonApiDto>>;

    public class GetPersonDataByDniQueryHandler: IRequestHandler<GetPersonDataByDniQuery, Result<PersonApiDto>>
    {
        public readonly IReniecService _reniecService;
        public GetPersonDataByDniQueryHandler(IReniecService getPersonDataRepository)
        {
            _reniecService = getPersonDataRepository;
        }

        public async Task<Result<PersonApiDto>> Handle(GetPersonDataByDniQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var person = await _reniecService.ConsultByDni(request.dni);
                if (person == null)
                {
                    return Result<PersonApiDto>.Failure("No se encontraron datos de la persona.");
                }
                return Result<PersonApiDto>.Success(person);
            }
            catch (Exception)
            {
                return Result<PersonApiDto>.Failure("Ocurrio un error al obtener los datos de la persona.");
            }
        }
    }

}
