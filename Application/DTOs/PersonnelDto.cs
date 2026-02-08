
namespace Application.DTOs
{
    public class PersonnelDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Role { get; set; }
        public required string Dni { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

    }
}
