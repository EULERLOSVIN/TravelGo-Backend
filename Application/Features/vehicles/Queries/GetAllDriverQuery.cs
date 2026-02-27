using Application.Common;
using Application.DTOs.vehicles;
using Application.Interfaces.vehicles;
using MediatR;

namespace Application.Features.vehicles.Queries
{
    public record GetAllDriverQuery() : IRequest<Result<List<PersonDto>>>;
    public class GetAllDriverQueryHandler : IRequestHandler<GetAllDriverQuery, Result<List<PersonDto>>>
    {
        private readonly IGetAllDriverRepository _getAllDriverRepository;
        public GetAllDriverQueryHandler(IGetAllDriverRepository getAllDriverRepository)
        {
            _getAllDriverRepository = getAllDriverRepository;
        }
        public async Task<Result<List<PersonDto>>> Handle(GetAllDriverQuery request, CancellationToken cancellation)
        {
            try
            {
                var success = await _getAllDriverRepository.GetDriversAsync();

                if (success == null)
                {
                    return Result<List<PersonDto>>.Failure("No se pudo completar el registro. Es posible que los datos ya existan o sean inválidos.");
                }
                return Result<List<PersonDto>>.Success(success);
            }
            catch (Exception)
            {
                return Result<List<PersonDto>>.Failure("Ocurrió un error inesperado al procesar el registro.");
            }

        }
    }
}
