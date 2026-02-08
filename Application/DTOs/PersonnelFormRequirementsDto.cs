namespace Application.DTOs
{
    public class PersonnelFormRequirementsDto
    {
        public List<RoleOfUserDto> Roles { get; set; } = new();
        public List<TypeDocumentDto> DocumentTypes { get; set; } = new();
    }
}
