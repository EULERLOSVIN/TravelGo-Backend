

namespace Application.DTOs
{
    public class RegisterUserDto
    {
        public required string  firstName { get; set; }
        public required string lastName { get; set; }
        public required int idRole { get; set; }
        public required int typeDocument { get; set; }
        public required string numberIdentityDocument { get; set; }
        public required string phoneNumber { get; set; }
        public required string password { get; set; }
    }
}

