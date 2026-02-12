using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.ManagementPersonnel.Commands
{
    public record EditUserCommand(EditPersonnelDto EditUserDto):IRequest<bool>;
    public class EditUserHandeler:IRequestHandler<EditUserCommand, bool>
    {
        private readonly IEditUserRepository _editUserRepository;
        public EditUserHandeler(IEditUserRepository editUserRepository)
        {
            _editUserRepository = editUserRepository;
        }
        public async Task<bool> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            return await _editUserRepository.EditUser(request.EditUserDto);
        }
    }
}
