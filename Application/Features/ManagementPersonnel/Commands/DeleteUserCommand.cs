using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ManagementPersonnel.Commands
{
    public record DeleteUserCommand(int Id): IRequest<bool>;
    public class DeleteUserHandler: IRequestHandler<DeleteUserCommand, bool>
    {
        public readonly IDeleteUserRepository _deleteUserRepository;
        public DeleteUserHandler(IDeleteUserRepository deleteUserRepository)
        {
            _deleteUserRepository = deleteUserRepository;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _deleteUserRepository.DeleteUser(request.Id);
        }
    }
}
